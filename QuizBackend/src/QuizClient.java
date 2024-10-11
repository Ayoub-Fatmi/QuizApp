
import java.io.*;
import java.net.*;

public class QuizClient implements Runnable {
    private Socket socket;
    private OutputStream os;
    private InputStream is;
    private InputStreamReader isr;
    private PrintWriter pw;
    private BufferedReader br;
    
    public String playerName;
    private int score;

    public QuizClient(Socket socket) throws IOException {
        this.socket = socket;
        this.os = socket.getOutputStream();
        this.pw = new PrintWriter(os, true);
        
        this.is = socket.getInputStream();
        this.isr = new InputStreamReader(is);
        this.br = new BufferedReader(isr);
        this.score = 0;
        
        
    }

    @Override
    public void run() {
        try {
            pw.println("Enter your name:");
            playerName = br.readLine();
            updateLead();
            if (QuizServer.getUsers().size() < 2) {
                pw.println("WAITING");
            }
            QuizServer.startGame();
            while (true) {
                String message = br.readLine();
                if (message.startsWith("ANSWER:")) {
                    String answer = message.substring(7);
                    String[] question = QuizServer.getQuestions().get(QuizServer.getRand());
                    
                    if (answer.equals(question[5])) {
                        score += 10;
                        pw.println("SCORE:" + playerName + ":" + score);
                        for (QuizClient user : QuizServer.getUsers()) {
                            if (user != this) {
                                user.pw.println("SCORE:" + playerName + ":" + score);
                                user.pw.println("TOO SLOW0");
                            }
                        }
                    } else {
                        for (QuizClient user : QuizServer.getUsers()) {
                            if (user != this) {
                                user.score += 10;
                                user.pw.println("SCORE:" + user.playerName + ":" + user.score);
                                user.pw.println("TOO SLOW1");
                            }
                            pw.println("SCORE:" + user.playerName + ":" + user.score);
                        }
                    }
                    QuizServer.incrementCurrentQuestionIndex();
                    if (QuizServer.getCurrentQuestionIndex() < 9) {
                        QuizServer.sendQuesToAll();
                    } else {
                        int maxScore = 0;
                        int minScore = 100;
                        String winner = "";
                        String loser = "";

                        for (QuizClient user : QuizServer.getUsers()) {
                            if (user.score > maxScore) {
                                maxScore = user.score;
                                winner = user.playerName;
                            }
                            if (user.score < minScore) {
                                minScore = user.score;
                                loser = user.playerName;
                            }
                        }

                        for (QuizClient user : QuizServer.getUsers()) {
                            user.pw.println("Quiz finished: Winner: " + winner + " with score " + maxScore + ", Loser: " + loser + " with score " + minScore);
                        }
                    }

                } else if (message.startsWith("CHAT:")) {
                    QuizServer.sendAll("CHAT:" + playerName + ": " + message.substring(5), this);
                }
            }
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            try {
                socket.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
            QuizServer.sendAll(playerName + " has left", this);
        }
    }

    public void sendMessage(String message) {
        pw.println(message);
    }

    private void updateLead() {
        for (QuizClient user : QuizServer.getUsers()) {
            user.sendMessage("SCORE:" + playerName + ":" + score);
        }
    }
    
}
