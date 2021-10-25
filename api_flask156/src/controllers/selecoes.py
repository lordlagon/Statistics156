import json
import pandas as pd
from flask_restx import Resource
from src.server.instance import server, mydb
from src.models.selecoes import *

app, api = server.app, server.api

ns = api.namespace('selecao', description='Api para selecao dos campos do statistics 156')

@ns.route('/assuntos')
class AssuntoList(Resource):
    @api.doc('list_assuntos')
    @api.marshal_list_with(assunto)
    def get(self):
        ASSUNTOS.clear()
        mydb.connect()
        query = "select fk_assunto, assunto from olap_156.dim_assunto where assunto is not null"
        df = pd.read_sql(query, mydb)
        result = df.to_json(orient="records")
        parsed = json.loads(result)
        for item in parsed:
            ASSUNTOS.append(item)
        mydb.close()
        return ASSUNTOS

@ns.route('/bairros')
class AssuntoList(Resource):
    @api.doc('list_bairros')
    @api.marshal_list_with(bairro)
    def get(self):
        mydb.connect()
        BAIRROS.clear()
        query = "select fk_bairro, bairro from olap_156.dim_bairro2 where bairro is not null;"
        df = pd.read_sql(query, mydb)
        result = df.to_json(orient="records")
        parsed = json.loads(result)
        print(parsed)
        for item in parsed:
            BAIRROS.append(item)
        mydb.close()
        return BAIRROS

@ns.route('/datas')
class AssuntoList(Resource):
    @api.doc('list_data')
    @api.marshal_list_with(data)
    def get(self):
        mydb.connect()
        DATAS.clear()
        query = "select fk_data, datacompleta, ano, diasemana, mes, trimestre, semestre from olap_156.dim_data where DataCompleta is not null"
        df = pd.read_sql(query, mydb)
        result = df.to_json(orient="records")
        parsed = json.loads(result)
        for item in parsed:
            DATAS.append(item)
        mydb.close()
        return DATAS

@ns.route('/faixa_etaria')
class FaixaEtariaList(Resource):
    @api.doc('list_faixa_etaria')
    @api.marshal_list_with(faixa_etaria)
    def get(self):
        mydb.connect()
        FAIXAS_ETARIAS.clear()
        query = "select fk_faixa_etaria_ibge, faixa_etaria FROM olap_156.dim_faixa_etaria_ibge where faixa_etaria is not null;"
        df = pd.read_sql(query, mydb)
        print(df)
        result = df.to_json(orient="records")
        print(result)
        parsed = json.loads(result)
        print(parsed)
        for item in parsed:
            FAIXAS_ETARIAS.append(item)
        mydb.close()
        return FAIXAS_ETARIAS

@ns.route('/generos')
class GeneroList(Resource):
    @api.doc('list_generos')
    @api.marshal_list_with(genero)
    def get(self):
        mydb.connect()
        GENEROS.clear()
        query = "select fk_genero, genero from olap_156.dim_genero where genero is not null"
        df = pd.read_sql(query, mydb)
        result = df.to_json(orient="records")
        parsed = json.loads(result)
        for item in parsed:
            GENEROS.append(item)
        mydb.close()
        return GENEROS

@ns.route('/regionais')
class RegionalList(Resource):
    @api.doc('list_regionais')
    @api.marshal_list_with(regional)
    def get(self):
        mydb.connect()
        REGIONAIS.clear()
        query = "select fk_regional, regional from olap_156.dim_regional where regional is not null"
        df = pd.read_sql(query, mydb)
        result = df.to_json(orient="records")
        parsed = json.loads(result)
        for item in parsed:
            REGIONAIS.append(item)
        mydb.close()
        return REGIONAIS

@ns.route('/subdivisao')
class SubdivisaoList(Resource):
    @api.doc('list_subdivisao')
    @api.marshal_list_with(subdivisao)
    def get(self):
        mydb.connect()
        SUBDIVISOES.clear()
        query = "select fk_subdivisao, subdivisao from olap_156.dim_subdivisao2 where subdivisao is not null"
        df = pd.read_sql(query, mydb)
        result = df.to_json(orient="records")
        parsed = json.loads(result)
        for item in parsed:
            SUBDIVISOES.append(item)
        mydb.close()
        return SUBDIVISOES

@ns.route('/tipos')
class TipoList(Resource):
    @api.doc('list_types')
    @api.marshal_list_with(tipo)
    def get(self):
        mydb.connect()
        TIPOS.clear()
        query = "select fk_tipo, tipo from olap_156.dim_tipo where tipo is not null"
        df = pd.read_sql(query, mydb)
        result = df.to_json(orient="records")
        parsed = json.loads(result)
        for item in parsed:
            TIPOS.append(item)
        mydb.close()
        return TIPOS
