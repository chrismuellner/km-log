﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorInputFile;
using KmLog.Server.Blazor.Services;
using KmLog.Server.Blazor.Validation.Models;
using KmLog.Server.Dto;
using Microsoft.AspNetCore.Components;

namespace KmLog.Server.Blazor.Shared
{
    public partial class ImportCsvComponent
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        [Inject]
        private AppState State { get; set; }

        private MemoryStream _file;

        private IDictionary<string, string> Columns { get; set; } = new Dictionary<string, string>();
        private IDictionary<string, string> Indexes { get; set; } = new Dictionary<string, string>();

        private ImportModel Model { get; set; } = new ImportModel();

        // todo: warning for format? e.g. ;
        private async Task HandleSelection(IFileListEntry[] files)
        {
            try
            {
                var file = files.FirstOrDefault();
                if (file != null)
                {
                    _file = new MemoryStream();
                    await file.Data.CopyToAsync(_file);

                    _file.Position = 0;
                    var reader = new StreamReader(_file);
                    var header = await reader.ReadLineAsync();
                    var firstLine = await reader.ReadLineAsync();

                    var columns = header.Split(";").Select(str => str.Trim('\"'))
                        .Where(str => !string.IsNullOrWhiteSpace(str)).ToArray();
                    var values = firstLine.Split(";").Select(str => str.Trim('\"')).ToArray();
                    for (int i = 0; i < columns.Count(); i++)
                    {
                        Columns.Add(columns[i], values[i]);
                        Indexes.Add(columns[i], i.ToString());
                    }

                    Model = new ImportModel
                    {
                        LicensePlate = file.Name.ToLower()
                    };
                }
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Error selecting file!");
            }
        }

        private async Task FormSubmitted()
        {
            try
            {
                if (_file == null)
                {
                    return;
                }

                _file.Position = 0;
                var content = new MultipartFormDataContent
                {
                    { new ByteArrayContent(_file.GetBuffer()), "upload", "filename" },
                    { new StringContent(Indexes[Model.DateColumn]), nameof(RefuelEntryDto.Date)},
                    { new StringContent(Indexes[Model.TotalDistanceColumn]), nameof(RefuelEntryDto.TotalDistance) },
                    { new StringContent(Indexes[Model.AmountColumn]), nameof(RefuelEntryDto.Amount) },
                    { new StringContent(Indexes[Model.CostColumn]), nameof(RefuelEntryDto.Cost) },
                    { new StringContent(Indexes[Model.PricePerLiterColumn]), nameof(RefuelEntryDto.PricePerLiter) },
                    { new StringContent(Indexes[Model.TankStatusColumn]), nameof(RefuelEntryDto.TankStatus) },
                    { new StringContent(Model.LicensePlate), nameof(CarDto.LicensePlate) }
                };
                await HttpClient.PostAsync("api/car/csv", content);

                Model = new ImportModel();
                Columns.Clear();
                await _file.DisposeAsync();
                _file = null;

                await State.UpdateCars();
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Error importing file!");
            }
        }
    }
}
