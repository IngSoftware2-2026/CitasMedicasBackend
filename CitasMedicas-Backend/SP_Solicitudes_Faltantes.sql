-- =============================================
-- SPs FALTANTES - MÓDULO SOLICITUDES
-- Ejecutar en dbGestionCitas
-- =============================================

-- =============================================
-- 1. LISTAR SOLICITUDES PÚBLICAS (con filtros)
-- =============================================
CREATE OR ALTER PROCEDURE [Clinica].[SP_SolicitudesPublicas_Listar]
    @EstadoSolicitudId INT = NULL,
    @MedicoId          INT = NULL,
    @Desde             DATETIME2 = NULL,
    @Hasta             DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        sp.Solicitud_Id      AS SolicitudId,
        sp.NombrePaciente,
        sp.Telefono,
        sp.Email,
        sp.MedicoId,
        d.NombrePublico      AS Medico,
        d.DuracionDefaultMinutos,
        sp.FechaHoraInicio,
        sp.Motivo,
        sp.EstadoSolicitudId AS EstadoId,
        es.CodigoEstado,
        es.NombreEstado      AS Estado,
        sp.FechaCreacion
    FROM Clinica.tbSolicitudesPublicas sp
    INNER JOIN Clinica.tbDoctores d
        ON d.MedicoId = sp.MedicoId
    INNER JOIN Catalogos.tbEstadosSolicitud es
        ON es.EstadoSolicitudId = sp.EstadoSolicitudId
    WHERE
        (@EstadoSolicitudId IS NULL OR sp.EstadoSolicitudId = @EstadoSolicitudId)
        AND (@MedicoId IS NULL OR sp.MedicoId = @MedicoId)
        AND (@Desde   IS NULL OR sp.FechaHoraInicio >= @Desde)
        AND (@Hasta   IS NULL OR sp.FechaHoraInicio <= @Hasta)
    ORDER BY sp.FechaCreacion DESC;
END
GO

-- =============================================
-- 2. OBTENER SOLICITUD PÚBLICA POR ID
-- =============================================
CREATE OR ALTER PROCEDURE [Clinica].[SP_SolicitudesPublicas_ObtenerPorId]
    @SolicitudId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        sp.Solicitud_Id      AS SolicitudId,
        sp.NombrePaciente,
        sp.Telefono,
        sp.Email,
        sp.MedicoId,
        d.NombrePublico      AS Medico,
        d.DuracionDefaultMinutos,
        sp.FechaHoraInicio,
        sp.Motivo,
        sp.EstadoSolicitudId AS EstadoId,
        es.CodigoEstado,
        es.NombreEstado      AS Estado,
        sp.FechaCreacion
    FROM Clinica.tbSolicitudesPublicas sp
    INNER JOIN Clinica.tbDoctores d
        ON d.MedicoId = sp.MedicoId
    INNER JOIN Catalogos.tbEstadosSolicitud es
        ON es.EstadoSolicitudId = sp.EstadoSolicitudId
    WHERE sp.Solicitud_Id = @SolicitudId;
END
GO

-- =============================================
-- 3. CAMBIAR ESTADO DE SOLICITUD PÚBLICA
-- =============================================
CREATE OR ALTER PROCEDURE [Clinica].[SP_SolicitudesPublicas_CambiarEstado]
    @SolicitudId  INT,
    @CodigoEstado VARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF NOT EXISTS (
            SELECT 1 FROM Clinica.tbSolicitudesPublicas
            WHERE Solicitud_Id = @SolicitudId
        )
        BEGIN
            SELECT -1 AS CodeStatus, 'La solicitud no existe' AS MessageStatus;
            RETURN;
        END

        DECLARE @EstadoId INT;
        SELECT @EstadoId = EstadoSolicitudId
        FROM Catalogos.tbEstadosSolicitud
        WHERE CodigoEstado = @CodigoEstado;

        IF @EstadoId IS NULL
        BEGIN
            SELECT -2 AS CodeStatus, 'El código de estado indicado no existe' AS MessageStatus;
            RETURN;
        END

        UPDATE Clinica.tbSolicitudesPublicas
        SET EstadoSolicitudId = @EstadoId
        WHERE Solicitud_Id = @SolicitudId;

        SELECT 1 AS CodeStatus, 'Estado de solicitud actualizado correctamente' AS MessageStatus;

    END TRY
    BEGIN CATCH
        SELECT 0 AS CodeStatus, ERROR_MESSAGE() AS MessageStatus;
    END CATCH
END
GO

-- =============================================
-- 4. LISTAR SOLICITUDES DE USUARIO (con filtros)
-- =============================================
CREATE OR ALTER PROCEDURE [Clinica].[SP_SolicitudesCita_Listar]
    @EstadoId   INT = NULL,
    @MedicoId   INT = NULL,
    @PacienteId INT = NULL,
    @Desde      DATETIME2 = NULL,
    @Hasta      DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        sc.SolicitudId,
        sc.PacienteId,
        CONCAT(p.Nombres, ' ', ISNULL(p.Apellidos, '')) AS NombrePaciente,
        p.Telefono,
        p.Correo             AS Email,
        sc.MedicoId,
        d.NombrePublico      AS Medico,
        d.DuracionDefaultMinutos,
        sc.FechaHoraInicio,
        sc.DuracionMinutos,
        sc.Motivo,
        sc.EstadoId,
        es.CodigoEstado,
        es.NombreEstado      AS Estado,
        sc.FechaCreacion
    FROM Clinica.tbSolicitudesCita sc
    INNER JOIN Clinica.tbPacientes p
        ON p.PacienteId = sc.PacienteId
    INNER JOIN Clinica.tbDoctores d
        ON d.MedicoId = sc.MedicoId
    INNER JOIN Catalogos.tbEstadosSolicitud es
        ON es.EstadoSolicitudId = sc.EstadoId
    WHERE
        (@EstadoId   IS NULL OR sc.EstadoId   = @EstadoId)
        AND (@MedicoId   IS NULL OR sc.MedicoId   = @MedicoId)
        AND (@PacienteId IS NULL OR sc.PacienteId = @PacienteId)
        AND (@Desde      IS NULL OR sc.FechaHoraInicio >= @Desde)
        AND (@Hasta      IS NULL OR sc.FechaHoraInicio <= @Hasta)
    ORDER BY sc.FechaCreacion DESC;
END
GO

-- =============================================
-- 5. OBTENER SOLICITUD DE USUARIO POR ID
-- =============================================
CREATE OR ALTER PROCEDURE [Clinica].[SP_SolicitudesCita_ObtenerPorId]
    @SolicitudId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        sc.SolicitudId,
        sc.PacienteId,
        CONCAT(p.Nombres, ' ', ISNULL(p.Apellidos, '')) AS NombrePaciente,
        p.Telefono,
        p.Correo             AS Email,
        sc.MedicoId,
        d.NombrePublico      AS Medico,
        d.DuracionDefaultMinutos,
        sc.FechaHoraInicio,
        sc.DuracionMinutos,
        sc.Motivo,
        sc.EstadoId,
        es.CodigoEstado,
        es.NombreEstado      AS Estado,
        sc.FechaCreacion
    FROM Clinica.tbSolicitudesCita sc
    INNER JOIN Clinica.tbPacientes p
        ON p.PacienteId = sc.PacienteId
    INNER JOIN Clinica.tbDoctores d
        ON d.MedicoId = sc.MedicoId
    INNER JOIN Catalogos.tbEstadosSolicitud es
        ON es.EstadoSolicitudId = sc.EstadoId
    WHERE sc.SolicitudId = @SolicitudId;
END
GO

-- =============================================
-- 6. CAMBIAR ESTADO DE SOLICITUD DE USUARIO
-- =============================================
CREATE OR ALTER PROCEDURE [Clinica].[SP_SolicitudesCita_CambiarEstado]
    @SolicitudId  INT,
    @CodigoEstado VARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF NOT EXISTS (
            SELECT 1 FROM Clinica.tbSolicitudesCita
            WHERE SolicitudId = @SolicitudId
        )
        BEGIN
            SELECT -1 AS CodeStatus, 'La solicitud no existe' AS MessageStatus;
            RETURN;
        END

        DECLARE @EstadoId INT;
        SELECT @EstadoId = EstadoSolicitudId
        FROM Catalogos.tbEstadosSolicitud
        WHERE CodigoEstado = @CodigoEstado;

        IF @EstadoId IS NULL
        BEGIN
            SELECT -2 AS CodeStatus, 'El código de estado indicado no existe' AS MessageStatus;
            RETURN;
        END

        UPDATE Clinica.tbSolicitudesCita
        SET EstadoId = @EstadoId
        WHERE SolicitudId = @SolicitudId;

        SELECT 1 AS CodeStatus, 'Estado de solicitud actualizado correctamente' AS MessageStatus;

    END TRY
    BEGIN CATCH
        SELECT 0 AS CodeStatus, ERROR_MESSAGE() AS MessageStatus;
    END CATCH
END
GO
