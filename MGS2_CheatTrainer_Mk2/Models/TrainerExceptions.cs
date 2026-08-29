using System;

namespace MGS2_CheatTrainer_V2.Models;

public class TrainerException(string message) : Exception(message);

public class SquelchableTrainerException(string message) : Exception(message);