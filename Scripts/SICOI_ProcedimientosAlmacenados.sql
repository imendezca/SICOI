CREATE OR ALTER PROCEDURE PA_NuevoConsecutivo @P_IDConsecutivo nvarchar(15), @P_Periodo nvarchar(4), @V_ConsecutivoNuevo int OUTPUT
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 21-10-2019
-- Descripción:       Procedimiento almacenado que brinda el nuevo consecutivo que va a ser
--					  asignado al nuevo registro que sea solicitado.
-- ENTRADAS:		  - @P_IDConsecutivo:    Identificador del consecutivo en la tabla CONSECUTIVO
--					  - @P_Periodo:          Año actual del consecutivo a solicitar.
--					  - @V_ConsecutivoNuevo: Variable que tiene que ser ingresada para
--											 devolver el nuevo consecutivo
--=============================================================
BEGIN
	SELECT @V_ConsecutivoNuevo = NumConsecutivo
	FROM CONSECUTIVO 
	WHERE   IDConsecutivo = @P_IDConsecutivo
		AND Periodo = @P_Periodo;
END;
GO

BEGIN
DECLARE @var int;	
EXEC PA_NuevoConsecutivo 'FAX', '2019', @V_ConsecutivoNuevo = @var OUTPUT;
SELECT @var cons;
END;
GO

CREATE OR ALTER PROCEDURE PA_InsertarFaxNuevo @P_CodDespacho nvarchar(4), @P_Asunto nvarchar(20), @P_Expediente nvarchar(14),
											  @P_Tipo nvarchar(10), @P_CantFolios int, @P_IDPrioridad int,
											  @P_IDCaracteristica int, @P_Resultado bit, @P_Actor nvarchar(30),
											  @P_Demandado nvarchar(30), @P_IDUsuarioIngreso nvarchar(20), @P_Observaciones nvarchar(250),
											  @P_NombreArchivo nvarchar(50)
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 18-10-2019
-- Descripción:       
--=============================================================
DECLARE
	@V_PeriodoActual	nvarchar(4),
	@V_ConsecutivoNuevo int;
BEGIN
	SET @V_PeriodoActual = CONVERT(nvarchar(4), YEAR(getDate()));
	EXEC PA_NuevoConsecutivo 'FAX', '2019', @V_ConsecutivoNuevo = @V_ConsecutivoNuevo OUTPUT;
	INSERT INTO FAX(Periodo, CodDespacho, ConsFax, Asunto, 
					FechaHoraIngreso, Expediente, Tipo, CantFolios, 
					IDPrioridad, IDCaracteristica, Resultado, Actor, 
					Demandado, IDUsuarioIngreso, Observaciones, NombreArchivo)
		   VALUES(@V_PeriodoActual, @P_CodDespacho, @V_ConsecutivoNuevo, @P_Asunto, 
				  getDate(), @P_Expediente, @P_Tipo, @P_CantFolios, 
				  @P_IDPrioridad, @P_IDCaracteristica, @P_Resultado, @P_Actor, 
				  @P_Demandado, @P_IDUsuarioIngreso, @P_Observaciones, @P_NombreArchivo);
END;
GO

EXEC PA_InsertarFaxNuevo '0001', 'Prueba #1', '190000010001PR', 'Recibido', 
						 10, 1, 1, 1, 'Fulano', 'Mengano',
						 'imendezca', 'Observaciones cachete', 'nombre vacilon';
GO

CREATE OR ALTER TRIGGER TRI_FAX
ON FAX
AFTER INSERT
AS
BEGIN
	UPDATE CONSECUTIVO
	SET NumConsecutivo = NumConsecutivo + 1
	WHERE IDConsecutivo = 'FAX';
END;

CREATE PROCEDURE PA_DESPACHO_InsertarNuevov @P_CodDespacho nvarchar(4), @P_Nombre nvarchar(50), @P_IDCircuito int = 2
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 18-10-2019
-- Descripción:       
--=============================================================
BEGIN
	INSERT INTO DESPACHO (CodDespacho, Nombre, IDCircuito)
	VALUES				 (@P_CodDespacho, @P_Nombre, @P_IDCircuito);
END
GO
