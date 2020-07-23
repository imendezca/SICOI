CREATE OR ALTER PROCEDURE PA_BITACORA_InsertaBitacora @P_IDUsuario nvarchar(20), @P_Accion nvarchar(20), @P_Descripcion nvarchar(250),
												    @P_Pantalla nvarchar(50)
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 06-02-2020
-- Descripción:       
--=============================================================
BEGIN
	INSERT INTO BITACORA(FechaHora, IDUsuario, Accion, Descripcion, Pantalla)
	VALUES			 (getDate(), @P_IDUsuario, @P_Accion, @P_Descripcion, @P_Pantalla);
END
GO


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

CREATE OR ALTER PROCEDURE PA_FAX_ConsultaFaxPorConsecutivoCompleto @P_Consecutivo nvarchar(12)
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 13-11-2019
-- Descripción:       
--=============================================================
BEGIN
	SELECT F.Periodo, F.CodDespacho, F.ConsFax, F.Asunto, 
		   F.FechaHoraIngreso, F.Expediente, F.Tipo, F.CantFolios, 
		   F.IDPrioridad, F.IDCaracteristica, F.Resultado, F.Actor, 
		   F.Demandado, F.IDUsuarioIngreso, F.UsuarioComprueba, F.FechaHoraRecibido,
		   F.IDUsuarioRecibido, F.IDConfirmacion, F.Observaciones, F.NombreArchivo, 
		   F.CodDespachoActual, C.Caracteristica CaracteristicaNombre, P.Prioridad PrioridadNombre,
		   D.Nombre,
		   F.Periodo + F.CodDespacho + RIGHT('0000' + Ltrim(Rtrim(ConsFax)),4) AS ConsecutivoCompleto
	FROM FAX F
	JOIN CARACTERISTICA C ON F.IDCaracteristica = C.IDCaracteristica
	JOIN PRIORIDAD P ON F.IDPrioridad = P.IDPrioridad
	JOIN DESPACHO D ON F.CodDespacho = D.CodDespacho
	WHERE F.Periodo + F.CodDespacho + RIGHT('0000' + Ltrim(Rtrim(F.ConsFax)),4) LIKE '%'+@P_Consecutivo+'%';
END;
GO

CREATE OR ALTER PROCEDURE PA_FAX_ConsultaFaxFiltrado @P_CodDespacho nvarchar(4), @P_FechaInicial DATE, @P_FechaFinal DATE,
													 @P_Consecutivo nvarchar(12), @P_Expediente nvarchar(12), @P_Asunto nvarchar(50),
													 @P_Prioridad int
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 06-12-2019
-- Descripción:       
--=============================================================
BEGIN
	SELECT F.Periodo, F.CodDespacho, F.ConsFax, F.Asunto, 
			F.FechaHoraIngreso, F.Expediente, F.CantFolios, F.IDPrioridad, 
			P.Prioridad PrioridadNombre, F.IDUsuarioRecibido, F.FechaHoraRecibido,
			F.CodDespachoActual,
			F.Periodo + F.CodDespacho + RIGHT('00000' + Ltrim(Rtrim(ConsFax)),4) AS ConsecutivoCompleto
	FROM FAX F
	JOIN PRIORIDAD P ON F.IDPrioridad = P.IDPrioridad
	WHERE (F.CodDespachoActual = @P_CodDespacho
		   OR @P_CodDespacho = '0176')
	  AND ((FechaHoraIngreso >= @P_FechaInicial OR @P_FechaInicial IS NULL) 
		   AND (FechaHoraIngreso < DATEADD(day,1,@P_FechaFinal) OR @P_FechaFinal IS NULL))
	  AND (F.Periodo + F.CodDespacho + RIGHT('0000' + Ltrim(Rtrim(F.ConsFax)),4) LIKE '%'+@P_Consecutivo+'%' OR @P_Consecutivo IS NULL)   
	  AND (Expediente LIKE '%'+@P_Expediente+'%' OR @P_Expediente IS NULL)
	  AND (Asunto LIKE '%'+@P_Asunto+'%' OR @P_Asunto IS NULL)
	  AND (@P_Prioridad = F.IDPrioridad OR @P_Prioridad IS NULL)
END;
GO

CREATE OR ALTER PROCEDURE PA_FAX_ConsultaFaxPorDespacho @P_CodDespacho nvarchar(4)
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 11-11-2019
-- Descripción:       
--=============================================================
BEGIN
	SELECT F.Periodo, F.CodDespacho, F.ConsFax, F.Asunto, 
			F.FechaHoraIngreso, F.Expediente, F.CantFolios, F.IDPrioridad, 
			P.Prioridad PrioridadNombre, F.IDUsuarioRecibido, F.FechaHoraRecibido,
			F.CodDespachoActual,
			F.Periodo + F.CodDespacho + RIGHT('00000' + Ltrim(Rtrim(ConsFax)),4) AS ConsecutivoCompleto
	FROM FAX F
	JOIN PRIORIDAD P ON F.IDPrioridad = P.IDPrioridad
	WHERE (F.CodDespachoActual = @P_CodDespacho
		   OR @P_CodDespacho = '0176');
END;
GO

CREATE OR ALTER PROCEDURE PA_FAX_EnviarFaxAlDespacho @P_ConsecutivoFax nvarchar(12)
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 28-11-2019
-- Descripción:       
--=============================================================
DECLARE
	@DespachoAEnviar nvarchar(4);
BEGIN
	SELECT @DespachoAEnviar = CodDespacho 
	FROM FAX 
	WHERE Periodo + CodDespacho + RIGHT('0000' + Ltrim(Rtrim(ConsFax)),4) = @P_ConsecutivoFax;

	IF(@DespachoAEnviar = NULL)
		BEGIN
			RETURN;
		END;

	UPDATE FAX
	SET	   CodDespachoActual = @DespachoAEnviar
	WHERE  Periodo + CodDespacho + RIGHT('0000' + Ltrim(Rtrim(ConsFax)),4) = @P_ConsecutivoFax;
END;
GO

CREATE OR ALTER PROCEDURE PA_FAX_InsertarFaxNuevo @P_CodDespacho nvarchar(4), @P_Asunto nvarchar(50), @P_Expediente nvarchar(14),
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
	EXEC PA_NuevoConsecutivo 'FAX', @V_PeriodoActual, @V_ConsecutivoNuevo = @V_ConsecutivoNuevo OUTPUT;

	SET @V_ConsecutivoCompleto = @V_PeriodoActual + @P_CodDespacho + RIGHT('00000' + Ltrim(Rtrim(@V_ConsecutivoNuevo)),4);

	INSERT INTO FAX(Periodo, CodDespacho, ConsFax, Asunto, 
					FechaHoraIngreso, Expediente, Tipo, CantFolios, 
					IDPrioridad, IDCaracteristica, Resultado, Actor, 
					Demandado, IDUsuarioIngreso, Observaciones, NombreArchivo,
					CodDespachoActual)
		   VALUES(@V_PeriodoActual, @P_CodDespacho, @V_ConsecutivoNuevo, @P_Asunto, 
				  getDate(), UPPER(@P_Expediente), @P_Tipo, @P_CantFolios, 
				  @P_IDPrioridad, @P_IDCaracteristica, @P_Resultado, @P_Actor, 
				  @P_Demandado, @P_IDUsuarioIngreso, @P_Observaciones, @V_ConsecutivoCompleto + '_' + @P_Expediente + '.pdf',
				  '0176');
	SET @P_ResultadoNuevoFax = @V_ConsecutivoCompleto;
END;
GO


BEGIN
	declare @V_Result nvarchar(13);
	EXEC PA_FAX_InsertarFaxNuevo '0176', 'Prueba #2', '190000010001PR', 'Recibido', 
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

CREATE OR ALTER PROCEDURE PA_FAX_RecibirFax @P_ConsecutivoFax nvarchar(12), @P_IDUsuario nvarchar(20)
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 03-12-2019
-- Descripción:       
--=============================================================
DECLARE
	@UsuarioQueRecibe nvarchar(20);
BEGIN
	SELECT @UsuarioQueRecibe = IDUsuarioRecibido
	FROM FAX
	WHERE  Periodo + CodDespacho + RIGHT('0000' + Ltrim(Rtrim(ConsFax)),4) = @P_ConsecutivoFax;
	IF @UsuarioQueRecibe IS NULL
		BEGIN
			UPDATE FAX
			SET	   IDUsuarioRecibido = @P_IDUsuario,
				   FechaHoraRecibido = getDate()
			WHERE  Periodo + CodDespacho + RIGHT('0000' + Ltrim(Rtrim(ConsFax)),4) = @P_ConsecutivoFax;
		END;
END;
GO


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

CREATE OR ALTER PROCEDURE PA_USUARIO_ConsultaRolesActuales @P_Usuario nvarchar(20)
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 20-11-2019
-- Descripción:       
--=============================================================
BEGIN
	SELECT R.IDRol, R.Nombre
	FROM USUARIO U
	JOIN usuario_rol UR ON U.IDUsuario = UR.IDUsuario
	JOIN ROL R ON UR.IDRol = R.IDRol
	WHERE    U.IDUsuario = @P_Usuario
		AND  GETDATE() BETWEEN UR.FechaInicio AND UR.FechaFinal;
END;
GO

EXEC PA_USUARIO_ConsultaRolesActuales 'imendezca';

CREATE OR ALTER PROCEDURE PA_USUARIO_ConsultarUsuario @P_Usuario nvarchar(20)
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 20-11-2019
-- Descripción:       
--=============================================================
BEGIN
	SELECT IDUsuario, CodDespacho, UltimoCambio
	FROM USUARIO
	WHERE IDUsuario = @P_Usuario;
END;
GO

EXEC PA_USUARIO_ConsultarUsuario 'imendezca';

CREATE OR ALTER PROCEDURE PA_USUARIO_IniciaSesion @P_Usuario nvarchar(20), @P_Contrasena nvarchar(200), @P_Resultado bit OUTPUT
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 20-11-2019
-- Descripción:       
--=============================================================
DECLARE
	@UsuarioExistente bit;
BEGIN
	SELECT @UsuarioExistente = COUNT(IDUsuario)
	FROM USUARIO
	WHERE    LOWER(IDUsuario)  = LOWER(@P_Usuario)
		AND  Contrasena = @P_Contrasena;
	
	SET @P_Resultado = @UsuarioExistente;
END;
GO

BEGIN
	DECLARE @Result bit;
	EXEC PA_USUARIO_IniciaSesion 'imendezca', '3ae45a7a8da22ad78dd5eab340043e09', @P_Resultado = @Result OUTPUT;
	SELECT @Result;
END;

CREATE OR ALTER PROCEDURE PA_USUARIO_TienePermisos @P_Usuario nvarchar(20), @P_Resultado bit OUTPUT
AS
--=============================================================
-- Autor:	          Isaac Santiago Méndez Castillo
-- Fecha de creación: 20-11-2019
-- Descripción:       
--=============================================================
DECLARE
	@UsuarioExistente bit;
BEGIN
	SELECT @UsuarioExistente = COUNT(U.IDUsuario)
	FROM USUARIO U
	JOIN usuario_rol UR ON U.IDUsuario = UR.IDUsuario
	JOIN ROL R ON UR.IDRol = R.IDRol
	WHERE    U.IDUsuario = @P_Usuario
		AND  GETDATE() BETWEEN UR.FechaInicio AND UR.FechaFinal;
	
	SET @P_Resultado = @UsuarioExistente;
END;
GO

BEGIN
	DECLARE @Result bit;
	EXEC PA_USUARIO_TienePermisos 'imendezca', @P_Resultado = @Result OUTPUT;
	SELECT @Result;
END;