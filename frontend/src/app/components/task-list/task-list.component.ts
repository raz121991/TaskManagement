import { Component, Input,Output, EventEmitter } from '@angular/core';
import { Task } from '../../models/task.model';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent {
  @Input() tasks: Task[] = [];
  @Output() edit = new EventEmitter<Task>();
  @Output() delete = new EventEmitter<number>();

  editTask(task: Task): void {
    this.edit.emit(task);
  }

  deleteTask(id: number): void {
    this.delete.emit(id);
  }
}