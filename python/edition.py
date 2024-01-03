class Edition:
    def __init__(self, title, url, price, discount, sales_type, data_id):
        self.title = title
        self.url = url
        self.price = price
        self.discount = discount
        self.sales_type = sales_type
        self.data_id = data_id

    def __str__(self):
        return "Title: " + self.title + "\n" + "URL: " + self.url + "\n" + "Price: " + self.price + "\n" + "Discount: " + self.discount + "Sales Type: " + self.sales_type + "\n" + "Data-ID: " + self.data_id