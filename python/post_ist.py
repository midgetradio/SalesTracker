from post_bot import POST_BOT
import datetime
from hyploggermod.hyplogger import HypLogger
import argparse

def handleArgs():
    parser = argparse.ArgumentParser()
    parser.add_argument("-t", "--test", help="use this flag to submit the post to r/Test_Posts", action="store_true")
    args = parser.parse_args()
    return args

# post_type = edit | submit
post_type = "submit"
# full name of post
thing_id = "t3_1951pjy"

args = handleArgs()

hlogger = HypLogger()

# Add post to reddit via bot
current_time = datetime.datetime.now()
print("==========================")
print("Adding Reddit Post")
print(current_time.strftime("%d %B, %Y %H:%M:%S"))
print("==========================")
print("Starting bot...")

post_bot = POST_BOT(thing_id, args.test)
is_success = True

try:
    post_bot.get_auth_token()
except Exception as error:
    is_success = False
    hlogger.log(is_success, 2, "Error getting auth token: " + str(error))
    
num = 0
try:    
    num = post_bot.get_new_titles()
except Exception as error:
    is_success = False
    hlogger.log(is_success, 2, "Error getting new titles: " + str(error))
    
if(num > 0):
    print(str(num) + " titles added.")
    try:
        post_bot.create_submit_post(post_type)
    except Exception as error:
        is_success = False
        hlogger.log(is_success, 2, "Error creating post: " + str(error))
else:
    print("No new titles to add.")
    hlogger.log(is_success, 2, "No new titles to add.")
    
if (is_success):
    hlogger.log(is_success, 2, "Succesfully added post.")
    
print("Complete.")
