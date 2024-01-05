from bs4 import BeautifulSoup
import requests
import datetime
import os
from edition import Edition
from dal import DAL

environment = os.getenv("ASPNETCORE_ENVIRONMENT")
dal = DAL(environment)
sales_types = dal.get_sales_types()
editions = []

current_time = datetime.datetime.now()
print("==========================")
print("Updating IST Sales Tracker")
print(current_time)
print("==========================")

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

        # for e in editions:
        #     print(e)
        #     print("---")
print(f"{len(editions)} items will be added to the ETL table.")
print("Inserting into etl table....")
dal.insert_editions(editions)
print("Updating database...")
dal.execute_usp_update_entries()
print("Truncating etl table...")
dal.execute_usp_truncate_etl()
print("Complete.")
