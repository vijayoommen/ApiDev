namespace ShoppingSite.Responses;

public class DataSavedResponse
{
    public bool Success { get; }
    public string Message { get;}
    public int RowsAffected { get; }
    public int? Id { get; set; } // Optional, can be used to return the ID of the saved entity

    public DataSavedResponse(bool success, string message, int rowsAffected = 0, int? id = 0)
    {
        Success = success;
        Message = message;
        RowsAffected = rowsAffected;
        Id = id;
    }

    public static DataSavedResponse CreateSuccess(string message = "Data saved successfully.", int rowsAffected = 0, int? id = 0)
    {
        return new DataSavedResponse(true, message, rowsAffected, id);
    }

    public static DataSavedResponse CreateFailure(string message = "Failed to save data.")
    {
        return new DataSavedResponse(false, message, 0, null);
    }
}