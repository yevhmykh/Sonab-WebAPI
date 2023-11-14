using Sonab.Core.Interfaces;

namespace Sonab.Core.Dto.Users.Subscriptions.Requests;

public record SubscribeRequest(int PublisherId) : RequestDto;