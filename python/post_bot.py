import requests
import requests.auth
import json
import os
from dal import DAL
import datetime

class POST_BOT:
    # initialze class with secrets
    def __init__(self, thing_id, is_test):
        dal = DAL()
        f = open(dal.secrets_path)
        secrets_json = json.load(f)
        f.close()
        
        self.is_test = is_test

        self.r_app_user = secrets_json['REDDIT']['r_app_user']
        self.r_app_secret =secrets_json['REDDIT']['r_app_secret']
        self.r_user = secrets_json['REDDIT']['r_user']
        self.r_pwd = secrets_json['REDDIT']['r_pwd']

        # define edit post and submit post urls
        self.edit_post_url = "https://oauth.reddit.com/api/editusertext"
        self.edit_post_thing_id = thing_id
        self.submit_post_url = "https://oauth.reddit.com/api/submit"

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
    def create_submit_post(self, submission_type):
        # set auth header
        headers = {"Authorization": ("bearer " + self.access_token), "User-Agent": "midgetradio"}

        # create post content
        post_text = "# Titles added since " + (datetime.date.today() - datetime.timedelta(days=7)).strftime("%d %B, %Y") + "\n" + "*(Omnis, Epics, Completes, Absolutes, Deluxes)*" + "\n" + "\n"

        for x in range (len(self.r_data["value"])):
            title = self.r_data["value"][x]["title"]
            url = self.r_data["value"][x]["url"]
            price = self.r_data["value"][x]["price"]
            discount = self.r_data["value"][x]["discount"]
            sale_type = self.r_data["value"][x]["salesType"]
            if(x == len(self.r_data["value"]) - 1):
                line = "[" + title + " " + "]" + "(" + url + ")" + " " + price + " " + discount + " " + sale_type
            else:
                line = "[" + title + " " + "]" + "(" + url + ")" + " " + price + " " + discount + " " + sale_type + "\\" + "\n"
            post_text += line
        
        post_text += "\\"
        post_text += "\n"
        post_text += "\\"
        post_text += "\n"
        post_text += "For a complete list of sales visit the IST Sales Tracker site (updated hourly)." + "\\" + "\n"
        post_text += "[IST Sales Tracker](https://salestracker.thehyperborean.net/)"
        post_text += "\\"
        post_text += "\n"
        post_text += "[InstockTrades](https://www.instocktrades.com/)"

        response_data = { "success": False }

        # edit an existing post
        if(submission_type == "edit"):
            post_data = {"thing_id": self.edit_post_thing_id, "text": post_text}
            response = requests.post(self.edit_post_url, headers=headers, data=post_data)
            response_data = response.json()
        
        # submit a new post
        if(submission_type == "submit"):
            post_data = None
            if (self.is_test):
                post_data = {"sr": "Test_Posts", "title":"IST Sales - Weekly Update", "text": post_text, "kind": "self"}
            else:
                post_data = {"sr": "Test_Posts", "title":"IST Sales - Weekly Update", "text": post_text, "flair_id": "4f8e0f16-d2bf-11eb-b960-0e495f026799", "kind": "self"}
            response = requests.post(self.submit_post_url, headers=headers, data=post_data)
            response_data = response.json()

        if(response_data["success"] != True):
            print("Failed to add post.")
            print(str(response_data["jquery"]))
            raise Exception("Failed to add post: " + str(response_data["jquery"]))
        else:
            print("Post added successfully.")
