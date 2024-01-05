import requests
import requests.auth
import json
import os
from dal import DAL



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

post_text = """# Title
- item 1
- item 2 \\
[Sales Tracker](https://salestracker.thehyperborean.net/) \\
[Sales Tracker2](https://salestracker.thehyperborean.net/) \\
"""

post_lines = ""
for x in range (0,3):
    line = "[Sales Tracker " + str(x) + "]" + "(https://salestracker.thehyperborean.net/)" + "\\" + "\n"
    post_lines += line

# print(post_lines)

# quit()

post_data = {"sr": "sandboxtest", "title":"test-post", "text": post_lines, "kind": "self"}

headers = {"Authorization": ("bearer " + access_token), "User-Agent": "midgetradio"}
response = requests.post("https://oauth.reddit.com/api/submit", headers=headers, data=post_data)



print(response.json())
