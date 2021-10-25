from flask_restx import fields
from src.server.instance import server

api = server.api

assunto = api.model('assunto', {
    'fk_assunto': fields.String(required=True, description='fk do assunto'),
    'assunto': fields.String(required=True, description='nome do assunto')
})

bairro = api.model('bairro', {
    'fk_bairro': fields.String(required=True, description='fk do bairro'),
    'bairro': fields.String(required=True, description='nome do bairro')
})

data = api.model('data', {
    'fk_data': fields.String(required=True, description='fk da data'),
    'datacompleta': fields.String(required=True, description='datetime da data'),
    'ano': fields.String(description='ano'),
    'mes': fields.String(description='mes'),
    'diasemana': fields.String(description='dia da semana'),
    'trimestre': fields.String(description='trimestre'),
    'semestre': fields.String(description='trimestre'),
})

faixa_etaria = api.model('faixa_etaria', {
    'fk_faixa_etaria_ibge': fields.String(required=True, description='fk do faixa_etaria'),
    'faixa_etaria': fields.String(required=True, description='nome do faixa_etaria')
})

genero = api.model('genero', {
    'fk_genero': fields.String(required=True, description='fk do genero'),
    'genero': fields.String(required=True, description='nome do genero')
})

regional = api.model('regional', {
    'fk_regional': fields.String(required=True, description='fk do regional'),
    'regional': fields.String(required=True, description='nome do regional')
})

subdivisao = api.model('subdivisao', {
    'fk_subdivisao': fields.String(required=True, description='fk do subdivisao'),
    'subdivisao': fields.String(required=True, description='nome do subdivisao')
})


tipo = api.model('tipo', {
    'fk_tipo': fields.String(required=True, description='fk do Tipo'),
    'tipo': fields.String(required=True, description='nome do Tipo')
})

ASSUNTOS = []
BAIRROS = []
DATAS = []
FAIXAS_ETARIAS =[]
GENEROS = []
REGIONAIS = []
SUBDIVISOES = []
TIPOS = []
