from flask_restx import fields
from src.server.instance import server

api = server.api

assuntoBairroMes = api.model('assuntoBairroMes', {
    'bairro': fields.String(description='nome do Tipo'),
    'count': fields.Integer()
})

consulta = api.model('consulta', {
    'tipo': fields.String(required=True, description='nome do Tipo'),
    'mes': fields.String(description='Mês das solicitações'),
    'ano': fields.Integer(),
    'count': fields.Integer()
})



QUANTIDADES = []
anos = {2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021}

countAno = api.model('countano', {
    'tipo': fields.String(description='nome do Tipo'),
    'mes': fields.String(description='Mês das solicitações'),
    'ano': fields.Integer(),
    'count': fields.Integer()

})
COUNTANOS = []

varSolicitacaoMes = api.model('varsolicitacaomes', {
    'tipo': fields.String(description='nome do Tipo'),
    'mes': fields.String(description='Mês das solicitações'),
    'ano': fields.Integer(),
    'count': fields.Integer()

})
SOLICITACOESMES = []
meses = { "Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez" }

TOP10ASSUNTOS = []

topAssuntos = api.model('topassuntos', {
    'assunto': fields.String(description='nome do Assunto'),
    'ano': fields.Integer(),
    'count': fields.Integer()
})