class Helpers:
    def get_ano(consulta):
        return consulta.get('ano')
    def get_mes(consulta):
        return consulta.get('mes')
    def get_data(consulta):
        return consulta.get('datacompleta')

helper = Helpers()