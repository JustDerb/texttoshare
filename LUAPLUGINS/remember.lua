
----------------------------------------------------------------------------------
-- INCOMING MESSAGE API CALLS ====================================================
-- Get plugin arguments: 
--    GetMessageArgumentString()                                  [returns String]
-- Get plugin arguments as a list: 
--    GetMessageArgumentList()                             [returns Key/Value Map]
-- Get plugin command: 
--    GetMessageCommand()                                         [returns String]
-- Get sender ID:  
--    GetSenderId()                                               [returns String]
--
-- GROUP API CALLS ===============================================================
-- Get list of group users ID:  
--    GetUserIdList()                                      [returns Key/Value Map]
-- Get list of group moderators ID:  
--    GetModeratorIdList()                                 [returns Key/Value Map]
-- Get owner ID:  
--    GetOwnerId()                                                [returns String]
-- Get the group description:  
--    GetGroupDescription()                                       [returns String]
-- Get the group name:  
--    GetGroupName()                                              [returns String]
-- Get the group tag:  
--    GetGroupTag()                                               [returns String]
-- 
-- USER API CALLS ================================================================
-- Get the users first name
--    GetUserFirstName(userID)                                     [return String]
-- Get the users last name
--    GetUserLastName(userID)                                      [return String]
-- Get the users username
--    GetUsername(userID)                                          [return String]
-- Get whether the user is suppressed (will not recieve messages)
--    GetUserIsSuppressed(userID)                                 [return Boolean]
-- Get whether the user is banned (will not recieve messages)
--    GetUserIsBanned(userID)                                     [return Boolean]
-- Get the users phone carrier
--    GetUserCarrier(userID)                                       [return String]
--
-- PLUGIN API CALLS ==============================================================
-- Set plugin key/value: 
--    SetValue(key, value)                                           [returns nil]
-- Get plugin key/value: 
--    GetValue(key)                                               [returns String]
-- Set user key/value: 
--    SetValue(key, value, userId)                                   [returns nil]
-- Get user key/value: 
--    GetValue(key, userId)                                       [returns String]

-- EXTRA API CALLS ===============================================================
-- Send a text to a user
--    SendMessage(userId, message)                                   [returns nil]
-- Retrieves the body text of any website URL (must be less than 1 MB to download)
-- This function will return nil if it cannot find/complete the request
--    HTTPDownloadText(URL)                                        [return String]
----------------------------------------------------------------------------------

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
