﻿using Stripe.Checkout;
using HyperPlayServer.Models.Database.Entities;
using HyperPlayServer.Models.Database.Repositories;
using HyperPlayServer.Models.Mappers;

namespace HyperPlayServer.Services;

public class StripeService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly ReserveAndOrderMapper _gameOrderMapper;


    public StripeService(UnitOfWork unitOfWork, ReserveAndOrderMapper gameOrderMapper) { 
        _unitOfWork = unitOfWork;
        _gameOrderMapper = gameOrderMapper;
    }

    public async Task<SessionCreateOptions> EmbededCheckout(int userId, int reserveId)
    {
        User user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        Reserve reserve = await _unitOfWork.ReserveRepository.GetLastReserveByUserId(userId);


        if (reserve == null) {
            throw new KeyNotFoundException($"No hay reserva con este ID {reserveId}.");
        }

        if (user == null) {
            throw new KeyNotFoundException($"No hay usuario con este ID {userId}");
        }

        List<SessionLineItemOptions> newLineItems = _gameOrderMapper.ToListSessionLineItemOptions(reserve.ReserveDetails);

        SessionCreateOptions options = new SessionCreateOptions
        {
            UiMode = "embedded",
            Mode = "payment",
            PaymentMethodTypes = ["card"],
            LineItems = newLineItems,
            CustomerEmail = user.Email,
            RedirectOnCompletion = "never"
        };

        return options;
    }

    public async Task SetSessionIdReserve(string sessionId, int reserveId)
    {
        Reserve reserve = await _unitOfWork.ReserveRepository.GetReserveById(reserveId);

        if (reserve != null) { 
            reserve.SessionId = sessionId;
            await _unitOfWork.SaveAsync();
        }
    }

    public async Task<Session> GetSessionAsync(string sessionId)
    {
        try
        {
            SessionService sessionService = new SessionService();
            return await sessionService.GetAsync(sessionId);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error al obtener la sesión de Stripe con ID {sessionId}.", ex);
        }
    }

    public async Task<Session> SessionStatus(string sessionId)
    {
        SessionService sessionService = new SessionService();
        Session session = await sessionService.GetAsync(sessionId);

        return session;
    }

    public async Task<string> GetPaymentStatus(string sessionId)
    {
        Session session = await GetSessionAsync(sessionId);
        return session.PaymentStatus;
    }
}
