-- Put the sender in a temp variable
Sender = GetSenderId()

-- Grab our previous text from the database
previousText = GetValue("previousText", Sender)

-- First time? Cute.
if (previousText == nil) then
	-- Tell them we'll remember it
	SendMessage(Sender, "I'll remember: "..GetMessageArgumentString())
else
	-- Tell them what we remembered
	SendMessage(Sender, "You sent this last time: "..previousText)
end

-- Store the current message in the database
SetValue("previousText", GetMessageArgumentString(), Sender)
