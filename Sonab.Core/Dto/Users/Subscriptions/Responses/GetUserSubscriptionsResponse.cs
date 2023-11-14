using Sonab.Core.Interfaces;

namespace Sonab.Core.Dto.Users.Subscriptions.Responses;

public record GetUserSubscriptionsResponse(List<SubscriptionFullInfo> SubscriptionInfos) : ResponseDto;
