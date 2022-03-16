CREATE PROCEDURE [dbo].[spPolozkaKosiku_SmazPolozkyZKosikuUzivatele]
	@IdUzivatel nvarchar(450)
AS
	DELETE FROM 
		[dbo].[PolozkaKosiku] 
	WHERE 
			(@IdUzivatel = IdUzivatel);
	SELECT @@ROWCOUNT;