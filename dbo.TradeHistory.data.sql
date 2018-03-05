SET IDENTITY_INSERT [dbo].[TradeHistory] OFF
INSERT INTO dbo.TradeHistory(id,created,person_id,amount,ticker_id) VALUES
(GETDATE(),1,100,1),
(GETDATE(),1,100,3),
(GETDATE(),1,100,4),
(GETDATE(),1,100,6),
(GETDATE(),1,100,6),
(GETDATE(),1,100,4),
(GETDATE(),1,100,2),
(GETDATE(),1,100,1)