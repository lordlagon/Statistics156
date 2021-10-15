from flask import Flask
from flask_restplus import Api

class server():
    def __init__(self, ):
        self.app = Flask(__name__)
        self.api = Api(
            self.app,
            version='1.0',
            title='Api Statitics 156',
            description='Api Statitics 156',
            doc='/docs'
        )
    def run(self, ):
        self.app.run(debug=True)