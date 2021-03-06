USE [TIENDA]
GO
/****** Object:  StoredProcedure [dbo].[TSP_Respaldos]    Script Date: 09/30/2020 16:46:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<GBARRIOS>
-- Create date:  26/12/2014 10:54
-- Modify date:  08/04/2015 19:11
-- Description:	<Procedimiento almacenado para la aplicacion de consulta de respaldos>
-- =============================================
ALTER PROCEDURE [dbo].[TSP_Respaldos]
@Opcion SMALLINT, @Fecha SMALLDATETIME = NULL

AS
BEGIN
	SET NOCOUNT ON;
	IF @Fecha IS NULL
	SET @Fecha = GETDATE()
	
	IF @Opcion = 1
		BEGIN		
			DECLARE @vSucursal AS CHAR(3) = '000'
			
			SELECT *,CONVERT(BIT, 0) AS SELECCIONADO FROM SPIDERPUB.dbo.SERVERVIDORES WHERE NoSuc = @vSucursal OR @vSucursal = '000'
			AND TIPO = 'T' AND NOT NOMBRE = 'Centra Corpo' AND NoSuc NOT IN (299)
			ORDER BY NoSuc
		END
	ELSE IF @Opcion = 2 -- CONSULTAR LAS RUTAS DONDE SE GUARDAN LOS RESPALDOS DE BD
		BEGIN 	
			SELECT * FROM dbo.RESPALDO
		END
	ELSE IF @Opcion = 3 -- SE ACTUALIZAN TODAS LAS RUTAS DE RESPALDOS A LA UNIDAD D
		BEGIN
			UPDATE dbo.RESPALDO SET RutaActual = 'D:\Respaldos\Semanal\', RutaAnterior = 'D:\Respaldos\Semanal\' WHERE Tipo = 'S'
			
			UPDATE dbo.RESPALDO SET RutaActual = 'D:\Respaldos\Diarios\', RutaAnterior = 'D:\Respaldos\Diarios\' WHERE Tipo = 'D'
			
			UPDATE dbo.RESPALDO SET RutaActual = 'D:\Respaldos\Diarios\log\', RutaAnterior = 'D:\Respaldos\Diarios\log\' WHERE Tipo = 'T'
		END
	ELSE IF @Opcion = 4 -- CONSULTA LA EJECUCION DE LOS JOBS DE RESPALDOS TANTO DIARIOS COMO SEMANALES
		BEGIN
			DECLARE @F INT
			SELECT @F =CONVERT( VARCHAR(20),@Fecha ,112 )

SELECT DISTINCT SUBSTRING(@@SERVERNAME,4,3) AS sucursal,
			 j.name as 'JobName',
			 run_date AS runDate,
			 CASE WHEN h.run_status=1 THEN 'Exito'
			      ELSE
			 CASE WHEN h.run_status=0 THEN '** Falló'
			      ELSE
			 CASE WHEN h.run_status=4 THEN 'En Ejecución'
			      ELSE
			 CASE WHEN h.run_status=3 THEN 'Cancelado'
			      ELSE
			 CASE WHEN h.run_status=1 THEN 'Exito'
			      ELSE 'Reintentado'
			 END 
			 END
			 END
			 END
			 END AS Estatus,
			CASE WHEN h.run_status=0 THEN message ELSE '' END AS Mensaje
			From msdb.dbo.sysjobs j 
			INNER JOIN msdb.dbo.sysjobhistory h 
			 ON j.job_id = h.job_id 
			WHERE (H.run_date=@F AND (J.name LIKE 'Respaldo Dife%%'  OR j.name LIKE 'Respaldo Diario %' OR J.name LIKE 'Respaldo Semanal%%'))
			--AND h.run_status = 0
			AND j.enabled = 1 --Only Enabled Jobs
		END
END
