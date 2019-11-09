-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2019-10-21 19:36:45.323

-- tables
-- Table: BITACORA
CREATE TABLE BITACORA (
    ID int  NOT NULL IDENTITY(1, 1),
    FechaYHora datetime  NOT NULL,
    Servidor nvarchar(20)  NOT NULL,
    IDUsuario nvarchar(20)  NOT NULL,
    Accion nvarchar(20)  NOT NULL,
    Descripcion nvarchar(250)  NOT NULL,
    Actualiza nvarchar(1000)  NOT NULL,
    Pantalla nvarchar(50)  NOT NULL,
    CONSTRAINT BITACORA_pk PRIMARY KEY  (ID)
);

-- Table: CARACTERISTICA
CREATE TABLE CARACTERISTICA (
    IDCaracteristica int  NOT NULL,
    Caracteristica varchar(20)  NOT NULL,
    CONSTRAINT CARACTERISTICA_pk PRIMARY KEY  (IDCaracteristica)
);

-- Table: CIRCUITO
CREATE TABLE CIRCUITO (
    IDCircuito int  NOT NULL,
    Nombre nvarchar(50)  NOT NULL,
    CONSTRAINT CIRCUITO_pk PRIMARY KEY  (IDCircuito)
);

-- Table: CONSECUTIVO
CREATE TABLE CONSECUTIVO (
    IDConsecutivo nvarchar(15)  NOT NULL,
    Periodo nvarchar(4)  NOT NULL,
    NumConsecutivo int  NOT NULL,
    CONSTRAINT CONSECUTIVO_pk PRIMARY KEY  (IDConsecutivo)
);

-- Table: DESPACHO
CREATE TABLE DESPACHO (
    CodDespacho nvarchar(4)  NOT NULL,
    IDCircuito int  NOT NULL,
    Nombre nvarchar(50)  NOT NULL,
    CONSTRAINT DESPACHO_pk PRIMARY KEY  (CodDespacho)
);

-- Table: ERROR
CREATE TABLE ERROR (
    IDError int  NOT NULL IDENTITY(1, 1),
    Detalle nvarchar(1000)  NOT NULL,
    CONSTRAINT ERROR_pk PRIMARY KEY  (IDError)
);

-- Table: FAX
CREATE TABLE FAX (
    Periodo nvarchar(4)  NOT NULL,
    CodDespacho nvarchar(4)  NOT NULL,
    ConsFax int  NOT NULL,
    Asunto nvarchar(20)  NOT NULL,
    FechaHoraIngreso datetime  NOT NULL,
    Expediente nvarchar(14)  NOT NULL,
    Tipo nvarchar(10)  NOT NULL,
    CantFolios int  NOT NULL,
    IDPrioridad int  NOT NULL,
    IDCaracteristica int  NOT NULL,
    Resultado bit  NOT NULL,
    Actor nvarchar(30)  NOT NULL,
    Demandado nvarchar(30)  NOT NULL,
    IDUsuarioIngreso nvarchar(20)  NOT NULL,
    UsuarioComprueba nvarchar(20)  NULL,
    FechaHoraRecibido datetime  NULL,
    IDUsuarioRecibido nvarchar(20)  NULL,
    IDConfirmacion int  NULL,
    Observaciones nvarchar(250)  NOT NULL,
    NombreArchivo nvarchar(50)  NOT NULL,
	CodDespachoActual nvarchar(4)  NOT NULL,
    CONSTRAINT FAX_pk PRIMARY KEY  (Periodo,CodDespacho,ConsFax)
);

-- Table: PRIORIDAD
CREATE TABLE PRIORIDAD (
    IDPrioridad int  NOT NULL,
    Prioridad nvarchar(10)  NOT NULL,
    CONSTRAINT PRIORIDAD_pk PRIMARY KEY  (IDPrioridad)
);

-- Table: ROL
CREATE TABLE ROL (
    IDRol nvarchar(10)  NOT NULL,
    Nombre nvarchar(20)  NOT NULL,
    Descripcion nvarchar(50)  NOT NULL,
    CONSTRAINT ROL_pk PRIMARY KEY  (IDRol)
);

-- Table: USUARIO
CREATE TABLE USUARIO (
    IDUsuario nvarchar(20)  NOT NULL,
    CodDespacho nvarchar(4)  NOT NULL,
    Contrasena nvarchar(200)  NOT NULL,
    UltimoCambio date  NOT NULL,
    CONSTRAINT USUARIO_pk PRIMARY KEY  (IDUsuario)
);

-- Table: usuario_rol
CREATE TABLE usuario_rol (
    IDUsuario nvarchar(20)  NOT NULL,
    IDRol nvarchar(10)  NOT NULL,
    FechaInicio datetime  NOT NULL,
    FechaFinal datetime  NOT NULL,
    CONSTRAINT usuario_rol_pk PRIMARY KEY  (IDUsuario,IDRol)
);

-- foreign keys
-- Reference: DESPACHO_CIRCUITO (table: DESPACHO)
ALTER TABLE DESPACHO ADD CONSTRAINT DESPACHO_CIRCUITO
    FOREIGN KEY (IDCircuito)
    REFERENCES CIRCUITO (IDCircuito);

-- Reference: FAX_CARACTERISTICA (table: FAX)
ALTER TABLE FAX ADD CONSTRAINT FAX_CARACTERISTICA
    FOREIGN KEY (IDCaracteristica)
    REFERENCES CARACTERISTICA (IDCaracteristica);

-- Reference: FAX_DESPACHO (table: FAX)
ALTER TABLE FAX ADD CONSTRAINT FAX_DESPACHO
    FOREIGN KEY (CodDespacho)
    REFERENCES DESPACHO (CodDespacho);
	
ALTER TABLE FAX ADD CONSTRAINT FAX_DESPACHO_Actual
   FOREIGN KEY (CodDespachoActual)
   REFERENCES DESPACHO (CodDespacho);

-- Reference: FAX_PRIORIDAD (table: FAX)
ALTER TABLE FAX ADD CONSTRAINT FAX_PRIORIDAD
    FOREIGN KEY (IDPrioridad)
    REFERENCES PRIORIDAD (IDPrioridad);

-- Reference: FAX_USUARIO_COMPRUEBA (table: FAX)
ALTER TABLE FAX ADD CONSTRAINT FAX_USUARIO_COMPRUEBA
    FOREIGN KEY (UsuarioComprueba)
    REFERENCES USUARIO (IDUsuario);

-- Reference: FAX_USUARIO_INGRESO (table: FAX)
ALTER TABLE FAX ADD CONSTRAINT FAX_USUARIO_INGRESO
    FOREIGN KEY (IDUsuarioIngreso)
    REFERENCES USUARIO (IDUsuario);

-- Reference: FAX_USUARIO_RECIBIDO (table: FAX)
ALTER TABLE FAX ADD CONSTRAINT FAX_USUARIO_RECIBIDO
    FOREIGN KEY (IDUsuarioRecibido)
    REFERENCES USUARIO (IDUsuario);

-- Reference: USUARIO_DESPACHO (table: USUARIO)
ALTER TABLE USUARIO ADD CONSTRAINT USUARIO_DESPACHO
    FOREIGN KEY (CodDespacho)
    REFERENCES DESPACHO (CodDespacho);

-- Reference: usuario_rol_ROL (table: usuario_rol)
ALTER TABLE usuario_rol ADD CONSTRAINT usuario_rol_ROL
    FOREIGN KEY (IDRol)
    REFERENCES ROL (IDRol);

-- Reference: usuario_rol_USUARIO (table: usuario_rol)
ALTER TABLE usuario_rol ADD CONSTRAINT usuario_rol_USUARIO
    FOREIGN KEY (IDUsuario)
    REFERENCES USUARIO (IDUsuario);

-- End of file.

