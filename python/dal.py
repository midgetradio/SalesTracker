import pyodbc
import os
import json

class DAL:
    settings_json_filename = "settings.json"
    
    def __init__(self):
        self.env = os.environ["ASPNETCORE_ENVIRONMENT"]
        self.settings_json_filepath = ""
        self.settings_json = ""
        self.sql_id = ""
        self.sql_pwd = ""
        
        # get env variables
        try:
            self.sql_id = os.environ["SQL_UID"]
            self.sql_pwd = os.environ["SQL_PWD"]
        except:
            print("Unable to load sql pwd and id")
            raise IOError()
        
        if("Dev" in self.env):
            # open settings file
            try:
                settings_file = open(self.settings_json_filename)
                self.settings_json = json.load(settings_file)
            except:
                raise IOError("Unable to open settings file: " + self.settings_json_filename)
            self.connection_string = self.settings_json["ConnectionStrings"]["DevConnection"]

        else:
            # open settings file
            cwd = os.getcwd()
            try:
                self.settings_json_filepath = "/home/midgetradio/cron/salestracker/python/" + self.settings_json_filename
                settings_file = open(self.settings_json_filepath)
                self.settings_json = json.load(settings_file)
            except:
                raise IOError("Unable to open settings file: " + self.settings_json_filepath)
            
            self.connection_string = self.settings_json["ConnectionStrings"]["ProdConnection"]
        
        
        # replace uid and pwd values
        self.connection_string = self.connection_string.replace("{SQL_UID}", self.sql_id)
        self.connection_string = self.connection_string.replace("{SQL_PWD}", self.sql_pwd)
        self.connect = pyodbc.connect(self.connection_string)    
        
        # establish connection
        try:
            self.connect = pyodbc.connect(self.connection_string)
        except Exception as error:
            print("Unable to connect to database.")
            raise IOError("Unable to connect to database. " + str(error))
    
    def get_sales_types(self):
        cursor = self.connect.cursor()
        sql = "SELECT * FROM SaleTypes"
        cursor.execute(sql)
        sale_types = cursor.fetchall()
        return sale_types
    
    def insert_editions(self, editions):
        insert = "INSERT INTO EditionsETL(Title, URL, Price, Discount, SaleType, DataId) VALUES (?, ?, ?, ?, ?, ?)"
        values = []
        for edition in editions:
            values.append((edition.title, edition.url, edition.price, edition.discount, edition.sales_type, edition.data_id))

        cursor = self.connect.cursor()
        cursor.executemany(insert, values)
        cursor.commit()

    def execute_usp_update_entries(self):
        cursor = self.connect.cursor()
        sql = "exec dbo.usp_update_entries"
        cursor.execute(sql)
        cursor.commit()

    def execute_usp_truncate_etl(self):
        cursor = self.connect.cursor()
        sql = "exec dbo.usp_truncate_etl"
        cursor.execute(sql)
        cursor.commit()
    
    def secrets_replacer(self, original, secrets_json):
        replacement = ""
        first = False
        pause_replace = False
        index_start = -1
        index_end = -1
        for i in range(len(original)):
            if(original[i] == '%'):
                if(index_start == -1 and not first):
                    first = True
                    pause_replace = True
                    index_start = i
                else: 
                    first = False
                    index_end = i
                if(index_start < index_end):
                    index_start = index_start + 1
                    variable_name = original[index_start:index_end]
                    val = secrets_json["SQLSERVER"][variable_name]
                    replacement += val
                    first = False
                    index_start = -1
                    index_end = -1
                    pause_replace = False
            elif not pause_replace:
                replacement += original[i]
        
        return replacement