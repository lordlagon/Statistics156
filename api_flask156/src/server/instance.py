import mysql.connector
from flask import Flask
from flask_restx import Api
from werkzeug.middleware.proxy_fix import ProxyFix

class Server():
    def __init__(self, ):
        self.app = Flask(__name__)
        self.api = Api(self.app,
            version='1.0',
            title='Api Statitics 156',
            description='Api Statitics 156',
            doc='/',
        )
        self.app.wsgi_app = ProxyFix(self.app.wsgi_app)

    def run(self, ):
        self.app.run(
            debug=True
        )

server = Server()

mydb = mysql.connector.connect(
  host="localhost",
  port="3305",
  user="root",
  password="root",
  database="olap_156"
)
