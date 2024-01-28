from post_bot import POST_BOT

# post_type = edit | submit
post_type = "edit"
# full name of post
thing_id = "t3_1951pjy"

# Add post to reddit via bot
print("Starting bot...")
post_bot = POST_BOT(thing_id)
post_bot.get_auth_token()
num = post_bot.get_new_titles()
if(num > 0):
    print(str(num) + " titles added.")
    post_bot.create_submit_post(post_type)
else:
    print("No new titles to add.")
print("Complete.")
