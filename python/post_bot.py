import requests
import requests.auth
import json
import os
from dal import DAL
from datetime import date

# get secrets based on environment
environment = os.getenv("ASPNETCORE_ENVIRONMENT")
dal = DAL(environment)
f = open(dal.secretsPath)
secrets_json = json.load(f)
f.close()

r_app_user = secrets_json['REDDIT']['r_app_user']
r_app_secret =secrets_json['REDDIT']['r_app_secret']
r_user = secrets_json['REDDIT']['r_user']
r_pwd = secrets_json['REDDIT']['r_pwd']

# get auth token
client_auth = requests.auth.HTTPBasicAuth(r_app_user, r_app_secret)
post_data = {"grant_type": "password", "username": r_user, "password": r_pwd}
headers = {"User-Agent": "midgetradio"}

response = requests.post("https://www.reddit.com/api/v1/access_token", auth=client_auth, data=post_data, headers=headers)

response_data = response.json()
access_token = response_data['access_token']

# get new titles from sales tracker api
r = requests.get("https://salestracker.thehyperborean.net/api/getlatest")
r_data = r.json()

post_text = "# Titles added as of " + date.today().strftime("%d %B, %Y") + "\n"
for x in range (len(r_data["value"])):
    title = r_data["value"][x]["title"]
    url = r_data["value"][x]["url"]
    price = r_data["value"][x]["price"]
    if(x == len(r_data["value"]) - 1):
        discount = r_data["value"][x]["discount"]
    else:
        discount = r_data["value"][x]["discount"]
    sale_type = r_data["value"][x]["type"]
    line = "[" + title + " " + "]" + "(" + url + ")" + " " + price + " " + discount + " " + sale_type + "\\" + "\n"
    post_text += line

post_data = {"sr": "sandboxtest", "title":"IST Sales Update", "text": post_text, "kind": "self"}

headers = {"Authorization": ("bearer " + access_token), "User-Agent": "midgetradio"}
response = requests.post("https://oauth.reddit.com/api/submit", headers=headers, data=post_data)

print(response.json())
