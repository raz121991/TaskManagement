import { Injectable } from '@angular/core';
import { HttpClient,HttpErrorResponse  } from '@angular/common/http';
import { Observable,throwError  } from 'rxjs';
import { Task } from '../models/task.model';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiUrl = 'https://localhost:7273/api/tasks'; 

  constructor(private http: HttpClient) {}

  getTasks(): Observable<Task[]> {
    return this.http.get<Task[]>(this.apiUrl).pipe(
      catchError(this.handleError)
    );
  }

  addTask(task: Task): Observable<Task> {
    return this.http.post<Task>(this.apiUrl, task).pipe(
      catchError(this.handleError)
    );
  }

  updateTask(task: Task): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${task.id}`, task).pipe(
      catchError(this.handleError)
    );
  }

  deleteTask(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {

    console.error(`Server Error: ${error.status}, Message: ${error.message}`);

    // Customize error handling for specific status codes
    let errorMessage = 'An unknown error occurred!';
    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      errorMessage = `A client-side or network error occurred: ${error.error.message}`;
    } else {
      // Server-side error
      switch (error.status) {
        case 400:
          errorMessage = 'Bad Request. Please check your input.';
          break;
        case 404:
          errorMessage = 'The resource was not found.';
          break;
        case 500:
          errorMessage = 'Internal Server Error. Please try again later.';
          break;
        default:
          errorMessage = `Unexpected server error: ${error.message}`;
          break;
      }
    }
    return throwError(() => new Error(errorMessage));
  }
}