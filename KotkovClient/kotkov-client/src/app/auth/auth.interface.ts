export interface TokenResponse {
    access_Token: string;
}

export interface Account {
  login: string;
  password: string;
  role: string;
  userId: number;
}