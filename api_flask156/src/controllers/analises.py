import json
import pandas as pd
from flask_restx import Resource
from src.server.instance import server, mydb
from src.models.selecoes import *
from src.models.analises import *
from src.helpers import Helpers

app, api = server.app, server.api

ns = api.namespace('analises', description='Api das analises da central 156')


@ns.route('/assuntoBairroMes/<assunto>/<mes>/<ano>')
@api.param('<assunto>')
@api.param('<ano>')
@api.param('<mes>')
class GetbairroAssunto(Resource):
    @api.marshal_list_with(assuntoBairroMes)
    def get(self, assunto, mes, ano):
        mydb.connect()
        
        BAIRRO_POR_ASSUNTOS = []
        query = "SELECT T.fk_assunto, assunto, bairro, mes, ano FROM olap_156.fato_central156 C inner join dim_assunto T inner join dim_bairro2 B inner join dim_Data D where C.FK_bairro = B.FK_bairro and C.FK_Assunto = T.FK_Assunto and C.FK_Data = D.FK_Data and C.FK_assunto = '{}' and mes = '{}' and ano = '{}'".format(assunto, mes, ano)
        df = pd.read_sql(query, mydb)
        gdf = df.groupby(['bairro','assunto','ano']).size().reset_index(name='count')
        mdf = gdf.sort_values(by='count', ascending=False)
        result = mdf.to_json(orient="records")
        parsed = json.loads(result)
        for item in parsed:
            BAIRRO_POR_ASSUNTOS.append(item)
        mydb.close()
        
        return BAIRRO_POR_ASSUNTOS

@ns.route('/consulta/<tipo>/<mes>')
@api.param('<tipo>')
@api.param('<mes>')
@api.response(404, 'Not found')
class ConsultaList(Resource):
    @api.doc('list_consulta')
    @api.marshal_list_with(consulta)
    def get(self, tipo, mes):
        QUANTIDADES.clear
        for ano in anos:
            query = "SELECT tipo, ano, mes, count(qtd) as count FROM olap_156.fato_central156 C inner join dim_tipo T inner join dim_Data D where C.FK_Tipo = T.FK_Tipo and C.FK_Data = D.FK_Data and T.FK_Tipo = {} and D.Ano ={} and D.mes='{}'".format(tipo, ano, mes)
            df = pd.read_sql(query, mydb)
            #print(df)
            result = df.to_json(orient="records")
            parsed = json.loads(result)
            #print(parsed)
            for item in parsed:
                if item['tipo'] is not None:
                    QUANTIDADES.append(item)
        print(QUANTIDADES)
        QUANTIDADES.sort(key=Helpers.get_ano)
        return QUANTIDADES

@api.route('/countano/<tipo>/<mes>')
@api.param('<tipo>')
@api.param('<mes>')
@api.response(404, 'Not found')
class ConsultaCountAno(Resource):
    @api.doc('list_countAno')
    @api.marshal_list_with(countAno)
    def get(self, tipo, mes):
        
        COUNTANOS.clear()
        query = "SELECT T.FK_tipo, mes, ano FROM olap_156.fato_central156 C inner join dim_tipo T inner join dim_Data D where C.FK_Tipo = T.FK_Tipo and C.FK_Data = D.FK_Data and T.FK_Tipo = {}".format(tipo)
        df = pd.read_sql(query, mydb)
        for ano in anos:
            options = [mes]
            rdf = df.loc[(df['mes'].isin(options)) & (df['ano']==ano)]
            result = rdf.to_json(orient="records")
            parsed = json.loads(result)
            count = len(parsed)
            countAno = {'ano': ano, 'count': count, 'tipo': tipo, 'mes': mes}
            
            COUNTANOS.append(countAno)
        COUNTANOS.sort(key=Helpers.get_ano)
        return COUNTANOS
        

@api.route('/varsolicitacaomes')
class GetVarSolicitacaoMes(Resource):
    @api.doc('list_varSolicitacaoMes')
    @api.marshal_list_with(varSolicitacaoMes)
    def get(self):
        SOLICITACOESMES = []
        query = "SELECT T.fk_tipo, tipo, mes, ano, datacompleta FROM olap_156.fato_central156 C inner join dim_tipo T inner join dim_Data D where C.FK_Tipo = T.FK_Tipo and C.FK_Data = D.FK_Data"
        df = pd.read_sql(query, mydb)
        gdf = df.groupby(['tipo','mes','ano']).size().reset_index(name='count')
        print(gdf)
        result = gdf.to_json(orient="records")
        parsed = json.loads(result)
        print(parsed)
        for item in parsed:
            SOLICITACOESMES.append(item)
        #varSolicitacaoMes = {'ano': ano, 'count': count, 'tipo': tipo, 'mes': mes}
        #print(varSolicitacaoMes)
        #     SOLICITACOESMES.append(varSolicitacaoMes)
        SOLICITACOESMES.sort(key=Helpers.get_ano)
        return SOLICITACOESMES



@api.route('/topassuntos')
class GetTopAssuntos(Resource):
    @api.doc('list_topassuntos')
    @api.marshal_list_with(topAssuntos)
    def get(self):
        TOP10ASSUNTOS = []
        query = "SELECT assunto, mes, ano FROM olap_156.fato_central156 C inner join dim_assunto A inner join dim_Data D where C.FK_Assunto = A.FK_Assunto and C.FK_Data = D.FK_Data"
        df = pd.read_sql(query, mydb)
        gdf = df.groupby(['assunto','ano']).size().reset_index(name='count')
        mdf = gdf.sort_values(by='count', ascending=False).head(10)
        print(mdf)
        result = mdf.to_json(orient="records")
        parsed = json.loads(result)
        print(parsed)
        for item in parsed:
            TOP10ASSUNTOS.append(item)
        return TOP10ASSUNTOS

@api.route('/topassuntos/bairro/<bairro>')
@api.param('<bairro>')
class GetTopAssuntos(Resource):
    @api.doc('list_topassuntos')
    @api.marshal_list_with(topAssuntos)
    def get(self, bairro):
        TOP10ASSUNTOS = []
        query = "SELECT assunto, subdivisao, mes, ano FROM olap_156.fato_central156 C inner join dim_assunto A inner join dim_subdivisao2 S inner join dim_Data D inner join dim_bairro2 B where B.Fk_bairro = C.Fk_bairro and S.Fk_subdivisao= C.fk_subdivisao and C.FK_Assunto = A.FK_Assunto and C.FK_Data = D.FK_Data"
        df = pd.read_sql(query, mydb)
        gdf = df.groupby(['assunto','subdivisao','ano']).size().reset_index(name='count')
        mdf = gdf.sort_values(by='count', ascending=False).head(10)
        print(mdf)
        result = mdf.to_json(orient="records")
        parsed = json.loads(result)
        print(parsed)
        for item in parsed:
            TOP10ASSUNTOS.append(item)
        return TOP10ASSUNTOS

BAIRRO_POR_ASSUNTOS = []

bairro_assunto = api.model('bairroAssunto', {
    'assunto': fields.String(description='nome do Assunto'),
    'count': fields.Integer()
})

@api.route('/bairroAssunto/<bairro>/<ano>')
@api.param('<bairro>')
@api.param('<ano>')
class GetbairroAssunto(Resource):
    @api.doc('list_bairroAssunto')
    @api.marshal_list_with(bairro_assunto)
    def get(self, bairro, ano):
        BAIRRO_POR_ASSUNTOS = []
        query = "SELECT assunto, bairro, mes, ano FROM olap_156.fato_central156 C inner join dim_assunto A inner join dim_bairro2 B inner join dim_Data D where B.Fk_bairro = C.fk_bairro and C.FK_Assunto = A.FK_Assunto and C.FK_Data = D.FK_Data and C.Fk_bairro = {} and ano={}".format(bairro, ano)
        df = pd.read_sql(query, mydb)
        gdf = df.groupby(['bairro','assunto','ano']).size().reset_index(name='count')
        mdf = gdf.sort_values(by='count', ascending=False).head(10)
        print(mdf)
        result = mdf.to_json(orient="records")
        parsed = json.loads(result)
        print(parsed)
        for item in parsed:
            BAIRRO_POR_ASSUNTOS.append(item)
        return BAIRRO_POR_ASSUNTOS

@api.route('/bairroAssunto')
class GetbairroAssunto(Resource):
    @api.doc('list_bairroAssunto')
    @api.marshal_list_with(bairro_assunto)
    def get(self):
        BAIRRO_POR_ASSUNTOS = []
        query = "SELECT assunto, bairro, mes, ano FROM olap_156.fato_central156 C inner join dim_assunto A inner join dim_bairro2 B inner join dim_Data D where B.Fk_bairro = C.fk_bairro and C.FK_Assunto = A.FK_Assunto and C.FK_Data = D.FK_Data and ano = 2021 and B.Fk_Bairro = '30'"
        df = pd.read_sql(query, mydb)
        gdf = df.groupby(['assunto','bairro','ano']).size().reset_index(name='count')
        mdf = gdf.sort_values(by='count', ascending=False).head(10)
        print(mdf)
        result = mdf.to_json(orient="records")
        parsed = json.loads(result)
        
        for item in parsed:
            BAIRRO_POR_ASSUNTOS.append(item)
        return BAIRRO_POR_ASSUNTOS

FAIXAS_ETARIAS_GENEROS = []

faixa_etaria_genero = api.model('faixaetariagenero', {
    'count': fields.Integer(),
    'faixa_etaria': fields.String(description='nome do Assunto'),
    'genero': fields.String(description='nome do Assunto')
})

@api.route('/faixaetariagenero/<assunto>')
@api.param('<assunto>')
class GetFaixaEtariaGenero(Resource):
    @api.doc('list_faixaEtariaGenero')
    @api.marshal_list_with(faixa_etaria_genero)
    def get(self, assunto):
        FAIXAS_ETARIAS_GENEROS = []
        query = "SELECT genero, faixa_etaria FROM olap_156.fato_central156 C inner join dim_assunto A inner join dim_genero G inner join dim_Data D inner join dim_faixa_etaria_ibge F where F.FK_Faixa_Etaria_ibge = C.FK_Faixa_Etaria_ibge and C.Fk_genero = G.fk_genero and C.FK_Assunto = A.FK_Assunto and C.FK_Data = D.FK_Data and F.FK_Faixa_Etaria_ibge != 20 and A.Fk_assunto = {}".format(assunto)
        df = pd.read_sql(query, mydb)
        gdf = df.groupby(['faixa_etaria','genero']).size().reset_index(name='count')
        mdf = gdf.sort_values(by='count', ascending=False)
        print(mdf)
        
        result = mdf.to_json(orient="records")
        parsed = json.loads(result)
        print(parsed)
        for item in parsed:
            FAIXAS_ETARIAS_GENEROS.append(item)
        return FAIXAS_ETARIAS_GENEROS

@api.route('/faixaetariagenero/regional/<regional>')
@api.param('<regional>')
class GetFaixaEtariaGenero(Resource):
    @api.doc('list_faixaEtariaGenero')
    @api.marshal_list_with(faixa_etaria_genero)
    def get(self, regional):
        FAIXAS_ETARIAS_GENEROS = []
        query = "SELECT genero, faixa_etaria FROM olap_156.fato_central156 C inner join dim_regional R inner join dim_genero G inner join dim_Data D inner join dim_faixa_etaria_ibge F where F.FK_Faixa_Etaria_ibge = C.FK_Faixa_Etaria_ibge and C.Fk_genero = G.fk_genero and C.FK_Regional = R.FK_Regional and C.FK_Data = D.FK_Data and F.FK_Faixa_Etaria_ibge != 20 and R.FK_Regional = {}".format(regional)
        df = pd.read_sql(query, mydb)
        gdf = df.groupby(['faixa_etaria','genero']).size().reset_index(name='count')
        mdf = gdf.sort_values(by='count', ascending=False)
        print(mdf)
        
        result = mdf.to_json(orient="records")
        parsed = json.loads(result)
        print(parsed)
        for item in parsed:
            FAIXAS_ETARIAS_GENEROS.append(item)
        return FAIXAS_ETARIAS_GENEROS

@api.route('/faixaetariagenero')
class GetFaixaEtariaGenero(Resource):
    @api.doc('list_faixaEtariaGenero')
    @api.marshal_list_with(faixa_etaria_genero)
    def get(self):
        FAIXAS_ETARIAS_GENEROS = []
        query = "SELECT genero, faixa_etaria FROM olap_156.fato_central156 C inner join dim_assunto A inner join dim_genero G inner join dim_Data D inner join dim_faixa_etaria_ibge F where F.FK_Faixa_Etaria_ibge = C.FK_Faixa_Etaria_ibge and C.Fk_genero = G.fk_genero and C.FK_Assunto = A.FK_Assunto and C.FK_Data = D.FK_Data and F.FK_Faixa_Etaria_ibge != 20 and ano = 2021"
        df = pd.read_sql(query, mydb)
        gdf = df.groupby(['faixa_etaria','genero']).size().reset_index(name='count')
        mdf = gdf.sort_values(by='count', ascending=False)
        print(mdf)
        result = mdf.to_json(orient="records")
        parsed = json.loads(result)
        print(parsed)
        for item in parsed:
            FAIXAS_ETARIAS_GENEROS.append(item)
        return FAIXAS_ETARIAS_GENEROS


DATASASSUNTOPORBAIRRO = []

data_assunto_por_bairro = api.model('dataAssuntoBairro', {
    'count': fields.Integer(),
    'datacompleta': fields.Date(),
})

@api.route('/dataAssuntoBairro/<assunto>/<bairro>')
@api.param('<assunto>')
@api.param('<bairro>')
class GetDataAssuntoBairro(Resource):
    @api.doc('list_dataAssuntoBairro')
    @api.marshal_list_with(data_assunto_por_bairro)
    def get(self, assunto, bairro):
        DATASASSUNTOPORBAIRRO = []
        query = "SELECT datacompleta, bairro, assunto FROM olap_156.fato_central156 C inner join dim_assunto A inner join dim_Data D inner join dim_bairro2 B where C.FK_assunto = A.FK_assunto and C.FK_Data = D.FK_Data and C.FK_Bairro = B.FK_Bairro and A.Fk_assunto = {} and b.FK_Bairro = {}".format(assunto, bairro)
        df = pd.read_sql(query, mydb)
        gdf = df.groupby(['datacompleta']).size().reset_index(name='count')
        mdf = gdf.sort_values(by='count', ascending=False)
        
        result = mdf.to_json(orient="records", date_format = "iso" )
        parsed = json.loads(result)
       
        for item in parsed:
            DATASASSUNTOPORBAIRRO.append(item)
        DATASASSUNTOPORBAIRRO.sort(key=Helpers.get_data)
        return DATASASSUNTOPORBAIRRO