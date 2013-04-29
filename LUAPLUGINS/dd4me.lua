Sender = GetSenderId()
ArgList = GetMessageArgumentList()
Count = 0
SetDD = false
DDKey = "CurrentDD"

if (ArgList ~= nil) then
    for key,value in ipairs(ArgList) do
        if (Count == 0) then
            if (value:upper() == "SET") then
                SetDD = true
            end
        elseif (Count == 1 and SetDD == true) then
            UserId = FindIdByUsername(value)
            if (UserId == nil) then
                SendMessage(Sender, "Cannot find username: "..value)
                return
            else
                SetValue(DDKey, value)
                SendMessage(Sender, "Setting DD to: "..value)
                return
            end
        end
        Count = Count + 1
    end
end

DDUsername = GetValue(DDKey)
if (DDUsername == nil) then
    SendMessage(Sender, "Their is no DD in the group!")
else
    UserId = FindIdByUsername(DDUsername)
    
    if (UserId == nil) then
        SendMessage(Sender, "The set DD is not in the group!")
    else
        SendMessage(Sender, "DD message sent to: "..GetUsername(UserId))
        SendMessage(UserId, GetUsername(Sender).." needs a DD!")
    end
end