import { Component, Input, Output, EventEmitter } from '@angular/core'
import { ChatSession } from '../../entities/chatsession'



@Component({
    selector: 'app-chat-session-list',
    templateUrl: './chat-session-list.component.html',
    styleUrls: ['./chat-session-list.component.css'],
    standalone: false
})
export class ChatSessionListComponent {
    @Input() sessions: ChatSession[] = [];
    @Input() selectedSession?: ChatSession
    @Output() sessionSelected = new EventEmitter<ChatSession>();
    @Output() newChat = new EventEmitter<void>();
    @Output() sessionDeleted = new EventEmitter<ChatSession>();

    onSessionClick(session: ChatSession) {
        this.sessionSelected.emit(session)
    }

    onNewChatClick() {
        this.newChat.emit()
    }

    onDeleteClick(session: ChatSession, event: Event) {
        event.stopPropagation();
        if(confirm('Are you sure you want to delete this chat session?')) {
            this.sessionDeleted.emit(session);
        }
    }

    isSelected(session: ChatSession): boolean {
        return this.selectedSession?.id === session.id
    }
}
