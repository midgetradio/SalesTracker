from bs4 import BeautifulSoup
import requests
import datetime
import os
from edition import Edition
from dal import DAL
from post_bot import POST_BOT
from hyploggermod.hyplogger import HypLogger

hlogger = HypLogger()
dal = None

try:
    dal = DAL()
except Exception as error:
    hlogger.log(False, 2, str(error))
    print(str(error))
    quit()
    
sales_types = dal.get_sales_types()

editions = []

current_time = datetime.datetime.now()
print("==========================")
print("Updating IST Sales Tracker")
print(current_time)
print("==========================")

is_success = True

try: 
    for sales_type in sales_types:
        type = sales_type[1]
        base_url = sales_type[2]
        page = requests.get(base_url)
        page_content = BeautifulSoup(page.content, "html.parser")
        page_div = page_content.find("input", class_="pagenumber")
        page_max = 0
        if(page_div != None):
            page_max = int(page_div['data-max'])
        page_count = 0

        print("Getting " + type + " sales...")

        while(page_count <= page_max):
            page_count = page_count + 1
            url = base_url + "?pg=" + str(page_count)

            page = requests.get(url)
            page_content = BeautifulSoup(page.content, "html.parser")
            issue_info = page_content.find_all("div", class_="item thumbplus")

            title = ""
            link = ""
            price = ""
            discount = ""
            data_id = ""

            for issue in issue_info:
                title_div = issue.find("div", class_="title")
                price_div = issue.find("div", class_="price")
                discount_div = issue.find("div", class_="discount")
                add_button = issue.find("button", class_="addtocart")

                if(title_div != None):
                    title = title_div.a.string
                    link = title_div.a.get('href')
                if(price_div != None):
                    price = price_div.string.strip()
                else:
                    price = ""
                if(discount_div != None):
                    discount = discount_div.next.strip()
                else:
                    discount = ""
                if(add_button != None):
                    data_id = add_button.get("data-id")
                else:
                    data_id = ""

                e = Edition(title, link, price, discount, sales_type[1], data_id)
                editions.append(e)
except Exception as error:
    is_success = False
    hlogger.log(is_success, 2, str(error))

# Insert data into ETL, execute stored procedure, truncate etl table
print(str(len(editions)) + " items will be added to the ETL table.")
print("Inserting into etl table....")
try:
    dal.insert_editions(editions)
except Exception as error:
    is_success = False
    hlogger.log(is_success, 2, "Error inserting editions into etl table. " + str(error))
    
print("Updating database...")
try:
    dal.execute_usp_update_entries()
except Exception as error:
    is_success = False
    hlogger.log(is_success, 2, "Error executing update sp. " + str(error))
    
print("Truncating etl table...")
try:
    dal.execute_usp_truncate_etl()
except Exception as error:
    is_success = False
    hlogger.log(is_success, 2, "Error excecuting truncate sp. " + str(error))
    
if (is_success):
    hlogger.log(is_success, 2, "Successfully added " + str(len(editions)) + " items to ETL table and updated the database.")
