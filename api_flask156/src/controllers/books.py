from flask import Flask
from flask_restplus import Api, Resource

from server.instance import server

app, api = server.app, server.api

books_db = [
    {'id':0, 'title':'Guerra e paz'},
    {'id':1, 'title':'Guerra e paz1'},
    {'id':2, 'title':'Guerra e paz2'},
    {'id':3, 'title':'Guerra e paz3'},
    {'id':4, 'title':'Guerra e paz4'},
]
@api.route('/books')
class BookList(Resource):
    def get(self, ):
        return books_db
