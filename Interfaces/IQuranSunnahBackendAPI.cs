﻿using Quran_Sunnah_BackendAI.Dtos;

namespace Quran_Sunnah_BackendAI.Interfaces
{
    public interface IQuranSunnahBackendAPI
    {
        bool Active { get; }
        Task<ResultData> SendRequestAsync(AskPayloadRequest payloadRequest);
    }
}