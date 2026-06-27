-- Procedimientos almacenados para la aplicación CasoEstudio
-- Archivo: CasoEstudio.Web/Database/StoredProcedures.sql
-- Aplique estos scripts en la base de datos CasoEstudio

CREATE OR ALTER PROCEDURE dbo.GetTickets
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		t.Consecutivo,
		t.PlacaVehiculo,
		t.FechaIngreso,
		t.MontoTotal,
		t.TipoVehiculo,
		v.DescripcionTipo
	FROM dbo.Tickets t
	LEFT JOIN dbo.TiposVehiculos v ON t.TipoVehiculo = v.TipoVehiculo
	ORDER BY t.Consecutivo;
END
GO

CREATE OR ALTER PROCEDURE dbo.InsertTicket
	@PlacaVehiculo VARCHAR(10),
	@FechaIngreso DATETIME,
	@MontoTotal DECIMAL(10,2),
	@TipoVehiculo INT,
	@ResultCode INT OUTPUT,
	@ResultMessage VARCHAR(250) OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		-- Inserción principal
		INSERT INTO dbo.Tickets (PlacaVehiculo, FechaIngreso, MontoTotal, TipoVehiculo)
		VALUES (@PlacaVehiculo, @FechaIngreso, @MontoTotal, @TipoVehiculo);

		SET @ResultCode = 0;
		SET @ResultMessage = 'OK';
	END TRY
	BEGIN CATCH
		SET @ResultCode = ERROR_NUMBER();
		SET @ResultMessage = ERROR_MESSAGE();
	END CATCH
END
GO
