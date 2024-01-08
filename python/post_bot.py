import requests
import requests.auth
import json
import os
from dal import DAL
from datetime import date

class POST_BOT:
    # initialze class with secrets
    def __init__(self):
        environment = os.getenv("ASPNETCORE_ENVIRONMENT")
        dal = DAL(environment)
        f = open(dal.secretsPath)
        secrets_json = json.load(f)
        f.close()

        self.r_app_user = secrets_json['REDDIT']['r_app_user']
        self.r_app_secret =secrets_json['REDDIT']['r_app_secret']
        self.r_user = secrets_json['REDDIT']['r_user']
        self.r_pwd = secrets_json['REDDIT']['r_pwd']

    def get_auth_token(self):
        # get auth token from reddit
        client_auth = requests.auth.HTTPBasicAuth(self.r_app_user, self.r_app_secret)
        post_data = {"grant_type": "password", "username": self.r_user, "password": self.r_pwd}
        headers = {"User-Agent": "midgetradio"}

        response = requests.post("https://www.reddit.com/api/v1/access_token", auth=client_auth, data=post_data, headers=headers)

        response_data = response.json()
        self.access_token = response_data['access_token']

    # get new titles from sales tracker api
    def get_new_titles(self):
        r = requests.get("https://salestracker.thehyperborean.net/api/getlatest")
        self.r_data = r.json()
        return len(self.r_data["value"])

    # create and submit post
    def create_submit_post(self):
        post_text = "# Titles added as of " + date.today().strftime("%d %B, %Y") + "\n"

        for x in range (len(self.r_data["value"])):
            title = self.r_data["value"][x]["title"]
            url = self.r_data["value"][x]["url"]
            price = self.r_data["value"][x]["price"]
            discount = self.r_data["value"][x]["discount"]
            sale_type = self.r_data["value"][x]["type"]
            if(x == len(self.r_data["value"]) - 1):
                line = "[" + title + " " + "]" + "(" + url + ")" + " " + price + " " + discount + " " + sale_type
            else:
                line = "[" + title + " " + "]" + "(" + url + ")" + " " + price + " " + discount + " " + sale_type + "\\" + "\n"
            post_text += line

        post_data = {"sr": "sandboxtest", "title":"IST Sales Update", "text": post_text, "kind": "self"}

        headers = {"Authorization": ("bearer " + self.access_token), "User-Agent": "midgetradio"}
        response = requests.post("https://oauth.reddit.com/api/submit", headers=headers, data=post_data)

        response_data = response.json()

        if(response_data["success"] != True):
            print("Failed to add post.")
            print(response_data["jquery"])
        else:
            print("Post added successfully.")
