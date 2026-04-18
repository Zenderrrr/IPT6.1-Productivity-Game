import { apiFetch } from '@/api/api.ts'
import type { User } from '@/types/user.ts'

export async function loginApi(usernameOrEmail: string, password: string): Promise<void> {
  return apiFetch('/auth/login', {
    method: 'POST',
    body: JSON.stringify({
      UsernameOrEmail: usernameOrEmail,
      Password: password
    })
  })
}

export async function registerApi(username: string, email: string, password: string) : Promise<number> {
  return apiFetch('/auth/register', {
    method: 'POST',
    body: JSON.stringify({
      Username : username,
      Email : email,
      Password: password
    })
  })
}

export async function logoutApi(): Promise<void> {
  return apiFetch('/auth/logout', {

  })
}

export async function meApi(token: string) : Promise<User> {
  return apiFetch<User>('/auth/me', {
    method: 'GET',
    headers: {
      Authorization: `Bearer ${token}`
    }
  })
}

export async function deleteUserApi(token: string) : Promise<void> {
  return apiFetch('/auth/me', {
    method: 'DELETE',
    headers: {
      Authorization: `BEARER ${token}`
    }
  })
}
