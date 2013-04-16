
-- Get arguments: MESSAGE.FULLARGS [returns String]
-- Get command: MESSAGE.COMMAND [returns String]
-- Get list of group users ID:  GetUserIdList() [returns Key/Value Map]
-- Get list of group moderators ID:  GetModeratorIdList() [returns Key/Value Map]
-- Get owner ID:  GetOwnerId() [returns String]
-- Get sender ID:  GetSenderId() [returns String]
-- Set plugin key/value: SetValue(key, value) [returns nil]
-- Get plugin key/value: GetValue(key) [returns String]
-- Set user key/value: SetValue(key, value, userId) [returns nil]
-- Get user key/value: GetValue(key, userId) [returns String]

-- Put the sender in a temp variable
Sender = GetSenderId()

-- Grab our previous text from the database
previousText = GetValue("previousText", Sender)

-- First time? Cute.
if (previousText == nil) then
	-- Tell them we'll remember it
	SendMessage(Sender, "I'll remember: "..MESSAGE.FULLARGS)
else
	-- Tell them what we remembered
	SendMessage(Sender, "You sent this last time: "..previousText)
end

-- Store the current message in the database
SetValue("previousText", MESSAGE.FULLARGS, Sender)
