import pyodbc
import os
import json

class DAL:
    prod_root = "/var/www/SalesTracker"
    dev_root = "D:\\Dev\\SalesTracker\\SalesTracker\\SalesTracker\\SalesTracker"
    
    def __init__(self, env):
        if(env == "Development"):
            settingsFile = open(self.dev_root + os.sep + "appsettings.Development.json")
            settingsJson = json.load(settingsFile)
            self.connectionString = settingsJson["ConnectionStrings"]["PythonConnection"]
            self.connectionString = self.env_variable_replacer(self.connectionString)

        else:
            settingsFile = open(self.prod_root + os.sep + "appsettings.Production.json")
            settingsJson = json.load(settingsFile)
            self.connectionString = settingsJson["ConnectionStrings"]["PythonConnection"]
            self.connectionString = self.env_variable_replacer(self.connectionString)
            
        self.connect = pyodbc.connect(self.connectionString)    
    
    def get_sales_types(self):
        cursor = self.connect.cursor()
        sql = "SELECT * FROM SaleTypes"
        cursor.execute(sql)
        sale_types = cursor.fetchall()
        return sale_types
    
    def insert_editions(self, editions):
        insert = "INSERT INTO EditionsETL(Title, URL, Price, SaleType) VALUES (?, ?, ?, ?)"
        values = []
        for edition in editions:
            values.append((edition.title, edition.url, edition.price, edition.sales_type))

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
    
    def env_variable_replacer(self, original):
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
                    environment_variable = original[index_start:index_end]
                    val = os.environ[environment_variable]
                    replacement += val
                    first = False
                    index_start = -1
                    index_end = -1
                    pause_replace = False
            elif not pause_replace:
                replacement += original[i]
        
        return replacement


