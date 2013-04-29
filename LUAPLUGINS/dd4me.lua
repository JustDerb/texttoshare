-- Store the sender ID
Sender = GetSenderId()
-- Grab our argument list
ArgList = GetMessageArgumentList()
-- Init the count for the arg list
Count = 0
-- Flag if we're setting the DD
SetDD = false
-- Our global group key
DDKey = "CurrentDD"

-- No args? Cool.
if (ArgList ~= nil) then
    -- Iterate through each arg
    for key,value in ipairs(ArgList) do
        -- First arg?
        if (Count == 0) then
            -- We setting the DD?
            if (value:upper() == "SET") then
                SetDD = true
            end
        -- Second arg (and setting?)
        elseif (Count == 1 and SetDD == true) then
            -- Try and find the username
            UserId = FindIdByUsername(value)
            -- We find it?
            if (UserId == nil) then
                -- Aww... too bad.
                SendMessage(Sender, "Cannot find username: "..value)
                return
            else
                -- Set our glorious DD!
                SetValue(DDKey, value)
                SendMessage(Sender, "Setting DD to: "..value)
                return
            end
        end
        -- Gotta keep count of the args!
        Count = Count + 1
    end
end

-- Get our stored DD
DDUsername = GetValue(DDKey)
-- Is it not set?
if (DDUsername == nil) then
    SendMessage(Sender, "Their is no DD in the group!")
else
    -- Get the ID of the username
    UserId = FindIdByUsername(DDUsername)
    
    -- Must've left the group... too bad.
    if (UserId == nil) then
        SendMessage(Sender, "The set DD is not in the group!")
    -- Send them the texts!
    else
        SendMessage(Sender, "DD message sent to: "..GetUsername(UserId))
        SendMessage(UserId, GetUsername(Sender).." needs a DD!")
    end
end
