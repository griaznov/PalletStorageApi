﻿using PalletStorage.Business.Models;

namespace PalletStorage.Repositories.Repositories;

public interface IBoxRepository
{
    Task<BoxModel?> CreateAsync(BoxModel box);
    Task<List<BoxModel>> GetAllAsync(int take, int skip = 0);
    Task<BoxModel?> GetAsync(int id);
    Task<BoxModel?> UpdateAsync(BoxModel box);
    Task<bool> DeleteAsync(int id);
    Task<int> CountAsync();
}
