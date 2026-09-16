import { Component, Input, inject, OnChanges } from '@angular/core';
import { Observable } from 'rxjs';
import { CommentService, Comment } from '../../Services/comment.service';
import { AsyncPipe, DatePipe } from '@angular/common';
import { LoginPageService } from '../../Services/login-page-service'
@Component({
  imports: [AsyncPipe, DatePipe],
  selector: 'app-comment-component',
  styleUrl: './comment-component.css',
  templateUrl: './comment-component.html',
})
export class CommentComponent implements OnChanges {

  private readonly commentService = inject(CommentService);
  private readonly loginService = inject(LoginPageService);

  @Input() bugRefId!: string;

  errorMessage = '';

  comments$!: Observable<Comment[]>;
  currentUser = this.loginService.getCurrentUser()?.username;
  ngOnChanges(): void {
    if (!this.bugRefId) {
      this.errorMessage = 'Bug reference ID is missing';
      return;
    }

    this.comments$ = this.commentService.getCommentById(this.bugRefId);
  }
}
