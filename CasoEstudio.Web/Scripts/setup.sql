-- Scripts para la base de datos CasoEstudio: procedimientos almacenados

CREATE PROCEDURE dbo.InsertTicket
@PlacaVehiculo varchar(10),
@FechaIngreso datetime,
@MontoTotal decimal(10,2),
@TipoVehiculo int,
@ResultCode int OUTPUT,
@ResultMessage varchar(250) OUTPUT
AS
BEGIN
  SET NOCOUNT ON;
  IF NOT EXISTS (SELECT 1 FROM dbo.TiposVehiculos WHERE TipoVehiculo = @TipoVehiculo)
  BEGIN
	SET @ResultCode = 1; SET @ResultMessage = 'Tipo de vehículo no existe.'; RETURN;
  END
  DECLARE @Count INT = (SELECT COUNT(1) FROM dbo.Tickets WHERE TipoVehiculo = @TipoVehiculo);
  IF @Count >= 2
  BEGIN
	SET @ResultCode = 2; SET @ResultMessage = 'Ya existen 2 tickets para este tipo de vehículo.'; RETURN;
  END
  INSERT INTO dbo.Tickets (PlacaVehiculo, FechaIngreso, MontoTotal, TipoVehiculo)
  VALUES (@PlacaVehiculo, @FechaIngreso, @MontoTotal, @TipoVehiculo);
  SET @ResultCode = 0; SET @ResultMessage = 'OK';
END
GO

CREATE PROCEDURE dbo.GetTickets
AS
BEGIN
  SELECT t.Consecutivo, t.PlacaVehiculo, t.FechaIngreso, t.MontoTotal, tv.DescripcionTipo
  FROM dbo.Tickets t
  JOIN dbo.TiposVehiculos tv ON t.TipoVehiculo = tv.TipoVehiculo
  ORDER BY t.FechaIngreso DESC;
END
GO
