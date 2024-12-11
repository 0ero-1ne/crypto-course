﻿namespace CourseProject.Messages
{
    public static class Messages
    {
        public const string KEY_ERROR = """
        Key must have size 32 characters
        Click button below the text box to generate strong key
        Allowed characters: 0-9A-Za-z@#!`'"\|/?$;:%^&*()-=_+.,~<>[]{}
        """;
        public const string ENCRYPT_INPUT_ERROR = "Choose file to encrypt and enter encryption key";
        public const string DECRYPT_INPUT_ERROR = "Choose file to decrypt and enter decryption key";
        public const string FILE_IS_NOT_FOUND = "File is not found. Choose the file again";
        public const string FILE_DECRYPTION_FORMAT_ERROR_TF = "Decryption file must have \"tf\" extension";
        public const string FILE_DECRYPTION_FORMAT_ERROR_SM4 = "Decryption file must have \"sm4\" extension";
        public const string ENCRYPTION_SUCCESS = "File was successfully encrypted";
        public const string ENCRYPTION_FAIL = "File was not encrypted";
        public const string DECRYPTION_SUCCESS = "File was successfully decrypted";
        public const string DECRYPTION_FAIL = "File was not decrypted";
    }
}