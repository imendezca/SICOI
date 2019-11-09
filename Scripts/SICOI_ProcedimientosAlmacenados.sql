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

CREATE OR ALTER PROCEDURE PA_FAX_InsertarFaxNuevo @P_CodDespacho nvarchar(4), @P_Asunto nvarchar(20), @P_Expediente nvarchar(14),
											      @P_Tipo nvarchar(10), @P_CantFolios int, @P_IDPrioridad int,
											      @P_IDCaracteristica int, @P_Resultado bit, @P_Actor nvarchar(30),
											      @P_Demandado nvarchar(30), @P_IDUsuarioIngreso nvarchar(20), @P_Observaciones nvarchar(250), 
											      @P_ResultadoNuevoFax nvarchar(12) OUTPUT
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 18-10-2019
-- Descripción:       
--=============================================================
DECLARE
	@V_PeriodoActual	nvarchar(4),
	@V_ConsecutivoNuevo int,
	@V_ConsecutivoCompleto nvarchar(13);
BEGIN
	SET @V_PeriodoActual = CONVERT(nvarchar(4), YEAR(getDate()));
	EXEC PA_NuevoConsecutivo 'FAX', '2019', @V_ConsecutivoNuevo = @V_ConsecutivoNuevo OUTPUT;

	SET @V_ConsecutivoCompleto = @V_PeriodoActual + @P_CodDespacho + RIGHT('00000' + Ltrim(Rtrim(@V_ConsecutivoNuevo)),4);

	INSERT INTO FAX(Periodo, CodDespacho, ConsFax, Asunto, 
					FechaHoraIngreso, Expediente, Tipo, CantFolios, 
					IDPrioridad, IDCaracteristica, Resultado, Actor, 
					Demandado, IDUsuarioIngreso, Observaciones, NombreArchivo)
		   VALUES(@V_PeriodoActual, @P_CodDespacho, @V_ConsecutivoNuevo, @P_Asunto, 
				  getDate(), @P_Expediente, @P_Tipo, @P_CantFolios, 
				  @P_IDPrioridad, @P_IDCaracteristica, @P_Resultado, @P_Actor, 
				  @P_Demandado, @P_IDUsuarioIngreso, @P_Observaciones, @V_ConsecutivoCompleto + '_' + @P_Expediente);
	SET @P_ResultadoNuevoFax = @V_ConsecutivoCompleto;
END;
GO


BEGIN
	declare @V_Result nvarchar(13);
	EXEC PA_FAX_InsertarFaxNuevo '0001', 'Prueba #2', '190000010001PR', 'Recibido', 
						 10, 1, 1, 1, 'Fulanito', 'Menganito',
						 'imendezca', 'Observaciones imaginate', 'nombre corrongo', @P_ResultadoNuevoFax = @V_Result OUTPUT;
	SELECT @V_Result;
END;

CREATE OR ALTER TRIGGER TRI_FAX
ON FAX
AFTER INSERT
AS
BEGIN
	UPDATE CONSECUTIVO
	SET NumConsecutivo = NumConsecutivo + 1
	WHERE IDConsecutivo = 'FAX';
END;

CREATE OR ALTER PROCEDURE PA_DESPACHO_InsertarNuevo @P_CodDespacho nvarchar(4), @P_Nombre nvarchar(50), @P_IDCircuito int = 2
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

CREATE OR ALTER PROCEDURE PA_ERROR_InsertarError @P_DetalleError nvarchar(1000)
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 30-10-2019
-- Descripción:       
--=============================================================
BEGIN
	INSERT INTO ERROR(FechaHora, Detalle)
	VALUES			 (getDate(), @P_DetalleError);
END
GO

CREATE OR ALTER PROCEDURE PA_DESPACHO_ListarDespachos
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 23-10-2019
-- Descripción:       
--=============================================================
BEGIN
	SET NOCOUNT ON;
	SELECT CodDespacho, Nombre, IDCircuito
	FROM DESPACHO;
END
GO

CREATE OR ALTER PROCEDURE PA_PRIORIDAD_ListarPrioridades
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 24-10-2019
-- Descripción:       
--=============================================================
BEGIN
	SET NOCOUNT ON;
	SELECT IDPrioridad, Prioridad
	FROM PRIORIDAD;
END
GO

CREATE OR ALTER PROCEDURE PA_CARACTERISTICA_ListarCaracteristicas
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 29-10-2019
-- Descripción:       
--=============================================================
BEGIN
	SET NOCOUNT ON;
	SELECT IDCaracteristica, Caracteristica
	FROM CARACTERISTICA;
END
GO





SELECT Periodo + RIGHT('00000' + Ltrim(Rtrim(ConsFax)),5)
FROM FAX
WHERE Periodo + RIGHT('00000' + Ltrim(Rtrim(ConsFax)),5) LIKE '%19%';