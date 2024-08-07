using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Procedimientos;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Entidades;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones
{
    public class DatoDeltaVRepositorio : OperacionalRepositorio<DatoDeltaV, object>, IDatoDeltaVRepositorio
    {
        public DatoDeltaVRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public async Task EliminarDatosDeltaVAsync(long? idRegistroSupervisor)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Registro.DatosDeltaV WHERE IdRegistroSupervisor=@IdRegistroSupervisor";
                await conexion.QueryAsync(sql, new { IdRegistroSupervisor = idRegistroSupervisor }, commandType: CommandType.Text);
            }
        }
        public async Task GuardarDatosDeltaVAsync(DatoDeltaV entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Registro.DatosDeltaV (Tanque,Nivel,Pres, Temp,Api,IdRegistroSupervisor,Actualizado) VALUES(@Tanque,@Nivel,@Pres,@Temp,@Api,@IdRegistroSupervisor,@Actualizado)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }
        public async Task<List<DatoDeltaV>> BuscarDatosDeltaVAsync(long idRegistroSupervisor)
        {
            var lista = new List<DatoDeltaV>();
            var sql = "SELECT * FROM Registro.DatosDeltaV WHERE IdRegistroSupervisor=@IdRegistroSupervisor";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<DatoDeltaV>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdRegistroSupervisor = idRegistroSupervisor
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task<List<DatoDeltaV>> BuscarDatosDeltaVPorDiaOperativoAsync(DateTime diaOperativo)
        {
            var lista = new List<DatoDeltaV>();
            var sql = "SELECT c.Producto,a.Tanque,a.Nivel FROM Registro.DatosDeltaV a INNER JOIN Reporte.RegistroSupervisor b ON a.IdRegistroSupervisor = b.IdRegistroSupervisor INNER JOIN Registro.Tanque c ON c.NroTanque = a.Tanque WHERE b.Fecha = CAST(@DiaOperativo AS DATE)";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<DatoDeltaV>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task<List<ListarProductosPorTipo>> BuscarDatosDeltaVPorDiaOperativoGlpFisProdAsync(DateTime diaOperativo,string producto)
        {
            var lista = new List<ListarProductosPorTipo>();
            var sql = "SELECT c.Producto,a.Tanque,a.Nivel,Reporte.ObtenerCorrecionVolumenGlpFisProd(@DiaOperativo,a.Tanque) as Inventario FROM Registro.DatosDeltaV a INNER JOIN Reporte.RegistroSupervisor b ON a.IdRegistroSupervisor = b.IdRegistroSupervisor INNER JOIN Registro.Tanque c ON c.NroTanque = a.Tanque WHERE b.Fecha = CAST(@DiaOperativo AS DATE) AND c.Producto=@Producto";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ListarProductosPorTipo>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo,
                        Producto = producto
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }



        public async Task EliminarVolumenDeltaVAsync(long? idRegistroSupervisor)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Registro.VolumeDeltaV WHERE IdRegistroSupervisor=@IdRegistroSupervisor";
                await conexion.QueryAsync(sql, new { IdRegistroSupervisor = idRegistroSupervisor }, commandType: CommandType.Text);
            }
        }

        public async Task GuardarVolumenDeltaVAsync(VolumenDeltaV entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Registro.VolumeDeltaV (NombreLote,Volumen,IdRegistroSupervisor,Actualizado) VALUES(@NombreLote,@Volumen,@IdRegistroSupervisor,@Actualizado)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public async Task GuardarVolumenTxtAsync(DatoComposicionUnnaEnergiaPromedio entidad)
        {
            var lista = new List<VolumenDeltaV>();
            var sql1 = "SELECT * FROM Propiedad.SuministradorComponente WHERE Suministrador = @Suministrador";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<VolumenDeltaV>(sql1,
                    new
                    {
                        Suministrador = entidad.componente
                    },
                    commandType: CommandType.Text).ConfigureAwait(false);
                lista = resultados.ToList();
            }

            if (lista.Count > 0)
            {
                var idComponente = lista[0].Id; 
                using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
                {
                    var sql = "INSERT INTO Registro.ComposicionUnnaEnergiaPromedio (IdDiaOperativo,IdComponente,PromedioComponente) VALUES(@IdDiaOperativo,@IdComponente,@PromedioComponente)";
                    await conexion.ExecuteAsync(sql, new
                    {
                        IdDiaOperativo = entidad.idDiaOperativo,
                        IdComponente = idComponente,
                        PromedioComponente = entidad.promedioComponente
                    }, commandType: CommandType.Text);
                }
            }
            
        }

        public async Task<List<VolumenDeltaV>> BuscarVolumenDeltaVAsync(long idRegistroSupervisor)
        {
            var lista = new List<VolumenDeltaV>();
            var sql = "SELECT * FROM Registro.VolumeDeltaV WHERE IdRegistroSupervisor=@IdRegistroSupervisor";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<VolumenDeltaV>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdRegistroSupervisor = idRegistroSupervisor
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task<List<VolumenDeltaV?>> ObtenerVolumenDeltaVAsync( DateTime? diaOperativo)
        {
            var lista = new List<VolumenDeltaV>();
            
            
             var   sql = "select A.*,b.Fecha from [Registro].[VolumeDeltaV] a inner join [Reporte].[RegistroSupervisor] b \r\non a.IdRegistroSupervisor = b.IdRegistroSupervisor\r\nwhere cast(b.Fecha as date) between CAST( (CAST((YEAR(CAST(@DiaOperativo AS DATE))*100)+MONTH(CAST(@DiaOperativo AS DATE)) AS VARCHAR(6)) + '01')   AS DATE) and CAST( (CAST((YEAR(CAST(@DiaOperativo AS DATE))*100)+MONTH(CAST(@DiaOperativo AS DATE)) AS VARCHAR(6)) + '15')   AS DATE) and a.NombreLote = 'LOTE IV'";
            
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<VolumenDeltaV>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo,// diaOperativo,
                        //NombreLote = nombreLote
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task<List<VolumenDeltaV?>> ObtenerVolumenDeltaVAsync2(DateTime? diaOperativo)
        {
            var lista = new List<VolumenDeltaV>();


            var sql = "select A.*,b.Fecha from [Registro].[VolumeDeltaV] a inner join [Reporte].[RegistroSupervisor] b \r\non a.IdRegistroSupervisor = b.IdRegistroSupervisor\r\nwhere cast(b.Fecha as date) between CAST( (CAST((YEAR(CAST(@DiaOperativo AS DATE))*100)+MONTH(CAST(@DiaOperativo AS DATE)) AS VARCHAR(6)) + '16')   AS DATE) and CAST( (CAST((YEAR(CAST(@DiaOperativo AS DATE))*100)+MONTH(CAST(@DiaOperativo AS DATE)) AS VARCHAR(6)) + CAST(DAY(EOMONTH(CAST(@DiaOperativo AS DATE))) AS VARCHAR(2)) )   AS DATE) and a.NombreLote = 'LOTE IV'";

            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<VolumenDeltaV>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo,// diaOperativo,
                        //NombreLote = nombreLote
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task EliminarProduccionDiariaMsAsync(long? idRegistroSupervisor)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Registro.ProduccionDiariaMs WHERE IdRegistroSupervisor=@IdRegistroSupervisor";
                await conexion.QueryAsync(sql, new { IdRegistroSupervisor = idRegistroSupervisor }, commandType: CommandType.Text);
            }
        }
        public async Task GuardarProduccionDiariaMsAsync(ProduccionDiariaMs entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Registro.ProduccionDiariaMs (Producto,MedidoresMasicos,Actualizado,IdRegistroSupervisor) VALUES(@Producto,@MedidoresMasicos,@Actualizado,@IdRegistroSupervisor)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }
        public async Task<List<ProduccionDiariaMs>> BuscarProduccionDiariaMsAsync(long idRegistroSupervisor)
        {
            var lista = new List<ProduccionDiariaMs>();
            var sql = "SELECT * FROM Registro.ProduccionDiariaMs WHERE IdRegistroSupervisor=@IdRegistroSupervisor";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ProduccionDiariaMs>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdRegistroSupervisor = idRegistroSupervisor
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }


        public async Task EliminarGnsVolumeMsYPcBrutoAsync(long? idRegistroSupervisor, string? tipo)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Registro.GnsVolumeMsYPcBruto WHERE IdRegistroSupervisor = @IdRegistroSupervisor AND Tipo=@Tipo";
                await conexion.QueryAsync(sql, new { IdRegistroSupervisor  = idRegistroSupervisor, Tipo = tipo}, commandType: CommandType.Text);
            }
        }
        public async Task GuardarGnsVolumeMsYPcBrutoAsync(GnsVolumeMsYPcBruto entidad)
        {           

            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Registro.GnsVolumeMsYPcBruto (Nombre,VolumeMs,PcBrutoRepCroma,Tipo,Actualizado,IdRegistroSupervisor) VALUES(@Nombre,@VolumeMs,@PcBrutoRepCroma,@Tipo,@Actualizado,@IdRegistroSupervisor)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }
        public async Task<List<GnsVolumeMsYPcBruto>> BuscarGnsVolumeMsYPcBrutoAsync(long idRegistroSupervisor,string? tipo)
        {
            var lista = new List<GnsVolumeMsYPcBruto>();
            var sql = "SELECT * FROM Registro.GnsVolumeMsYPcBruto WHERE IdRegistroSupervisor = @IdRegistroSupervisor AND Tipo=@Tipo";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<GnsVolumeMsYPcBruto>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdRegistroSupervisor = idRegistroSupervisor,
                        Tipo = tipo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }



        public async Task EliminarDatosCgnAsync(long? idRegistroSupervisor)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Registro.DatoCgn WHERE IdRegistroSupervisor = @IdRegistroSupervisor";
                await conexion.QueryAsync(sql, new { IdRegistroSupervisor = idRegistroSupervisor }, commandType: CommandType.Text);
            }
        }
        public async Task GuardarDatosCgnAsync(DatoCgn entidad)
        {

            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Registro.DatoCgn (Tanque,Centaje,Volumen,Actualizado,IdRegistroSupervisor) VALUES(@Tanque,@Centaje,@Volumen,@Actualizado,@IdRegistroSupervisor)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }
        public async Task<List<DatoCgn>> BuscarDatosCgnAsync(long idRegistroSupervisor)
        {
            var lista = new List<DatoCgn>();
            var sql = "SELECT * FROM Registro.DatoCgn WHERE IdRegistroSupervisor = @IdRegistroSupervisor";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<DatoCgn>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdRegistroSupervisor = idRegistroSupervisor
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }



        public async Task EliminarVolumenDeDespachoAsync(long? idRegistroSupervisor, string? tipo)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Registro.VolumenDespacho WHERE IdRegistroSupervisor = @IdRegistroSupervisor AND Tipo=@Tipo";
                await conexion.QueryAsync(sql, new { IdRegistroSupervisor = idRegistroSupervisor, Tipo = tipo }, commandType: CommandType.Text);
            }
        }
        public async Task GuardarVolumenDeDespachoAsync(VolumenDespacho entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Registro.VolumenDespacho (Tanque,Cliente,Placa,Volumen,Tipo,Actualizado,IdRegistroSupervisor) VALUES(@Tanque,@Cliente,@Placa,@Volumen,@Tipo,@Actualizado,@IdRegistroSupervisor)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }
        public async Task<List<VolumenDespacho>> BuscarVolumenDeDespachoAsync(long idRegistroSupervisor, string? tipo)
        {
            var lista = new List<VolumenDespacho>();
            var sql = "SELECT * FROM Registro.VolumenDespacho WHERE IdRegistroSupervisor = @IdRegistroSupervisor AND Tipo=@Tipo";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<VolumenDespacho>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdRegistroSupervisor = idRegistroSupervisor,
                        Tipo = tipo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }


        public async Task EliminarDespachoGlpEnvasadoAsync(long? idRegistroSupervisor)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Registro.DespachoGlpEnvasado WHERE IdRegistroSupervisor = @IdRegistroSupervisor";
                await conexion.QueryAsync(sql, new { IdRegistroSupervisor = idRegistroSupervisor}, commandType: CommandType.Text);
            }
        }
        public async Task GuardarDespachoGlpEnvasadoAsync(DespachoGlpEnvasado entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Registro.DespachoGlpEnvasado (Nombre,Envasado,Granel,Actualizado,IdRegistroSupervisor) VALUES(@Nombre,@Envasado,@Granel,@Actualizado,@IdRegistroSupervisor)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public async Task<List<DespachoGlpEnvasado>> BuscarDespachoGlpEnvasadoAsync(long idRegistroSupervisor)
        {
            var lista = new List<DespachoGlpEnvasado>();
            var sql = "SELECT * FROM Registro.DespachoGlpEnvasado WHERE IdRegistroSupervisor = @IdRegistroSupervisor";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<DespachoGlpEnvasado>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdRegistroSupervisor = idRegistroSupervisor
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }


    }
}
