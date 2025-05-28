using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClientApp.Helpers;
using ClientApp.Mappers;
using ClientApp.Messages;
using ClientApp.Models;
using ClientApp.Shared.DTOs.TransactionDto;
using ClientApp.Shared.Enums.Transaction;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientApp.Services;

public class TransactionService
{
    private readonly HttpService _httpService;
    public List<TransactionModel> Transactions { get; private set; } = new();

    public TransactionService(HttpService httpService)
    {
        _httpService = httpService;
        Console.WriteLine("TransactionService created and registering message handlers.");

        WeakReferenceMessenger.Default.Register<UserLoggedInMessage>(this, (_, _) =>
        {
            Console.WriteLine("UserLoggedInMessage received in TransactionService.");


            _ = LoadAsync(notify: true);
        });

        WeakReferenceMessenger.Default.Register<UserLoggedOutMessage>(this, (_, _) =>
        {
            Console.WriteLine("UserLoggedOutMessage received in TransactionService.");
            Clear();
        });
    }

    public async Task LoadAsync(TransactionQueryObject? query = null, bool notify = true)
    {
        try
        {
            query ??= new TransactionQueryObject
            {
                TransactionStatus = TransactionStatus.Completed,
                SortBy = TransactionSortBy.CreatedAt,
                IsDescending = true
            };

            var queryString = query.ToQueryString();
            var result = await _httpService.GetWithRouteAsync<List<TransactionDto>>($"/api/transaction{queryString}");

            if (result != null)
            {
                Transactions = result.ConvertAll(dto => dto.ToTransactionModel());
                Console.WriteLine($"Loaded {Transactions.Count} transactions.");

                if (notify)
                    WeakReferenceMessenger.Default.Send(new TransactionsLoadedMessage(Transactions));
            }
            else
            {
                Console.WriteLine("Transaction data response was null.");
                Transactions.Clear();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"TransactionService LoadAsync failed: {ex.Message}");
            Transactions.Clear();
        }
    }


    public async Task<TransactionDto?> VerifyAsync(string? transactionRef)
    {
        var transactionDto = await _httpService.PostAsync<object, TransactionDto>($"/api/transaction/verify/{transactionRef}", new { });


        if (transactionDto != null)
        {
            Console.WriteLine($"Verification of transaction \"{transactionRef}\" is successfull");
            return transactionDto;
        }

        Console.WriteLine("Verification Failed");
        return null;
    }

    public async Task<TransactionResultDto?> ProcessQrPayment(BasePaymentRequestDto? requestDto)
    {
        if (requestDto is QrPaymentRequestDto qrRequestDto)
        {
            var transactionResultDto = await _httpService.PostAsync<QrPaymentRequestDto, TransactionResultDto>(
                "/api/transaction/payment/qr/process", qrRequestDto);

            if (transactionResultDto != null)
            {
                Console.WriteLine($"Payment is successful thru QR code payment");
                return transactionResultDto;
            }
        }

        return null;
    }

    public async Task<TransactionRefDto?> GenerateTransactionAsync()
    {
        try
        {
            var transactionRefDto = await _httpService.PostAsync<object, TransactionRefDto>("/api/transaction", new());
            Console.WriteLine("Transaction generation successful.");
            return transactionRefDto;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to generate transaction: {ex.Message}");
            return null;
        }
    }

    public async Task<TransactionDto?> UpdateTransactionAsync(string transactionRef, UpdateTransactionRequestDto requestDto)
    {
        try
        {
            var updatedTransaction = await _httpService.PatchAsync<UpdateTransactionRequestDto, TransactionDto>($"/api/transaction/{transactionRef}", requestDto);
            Console.WriteLine($"Transaction {transactionRef} updated successfully.");
            return updatedTransaction;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update transaction {transactionRef}: {ex.Message}");
            return null;
        }
    }


    public void Clear()
    {
        Transactions.Clear();
        Console.WriteLine("Transaction data cleared.");
    }
}