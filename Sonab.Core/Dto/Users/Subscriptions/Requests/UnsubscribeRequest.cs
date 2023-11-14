using Sonab.Core.Interfaces;

namespace Sonab.Core.Dto.Users.Subscriptions.Requests;

public record UnsubscribeRequest(int PublisherId) : RequestDto;