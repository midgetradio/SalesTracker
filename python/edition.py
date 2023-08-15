class Edition:
    def __init__(self, title, url, price, sales_type):
        self.title = title
        self.url = url
        self.price = price
        self.sales_type = sales_type

    def __str__(self):
        return "Title: " + self.title + "\n" + "URL: " + self.url + "\n" + "Price: " + self.price + "\n" + "Sales Type: " + self.sales_type