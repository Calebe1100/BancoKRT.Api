#!/bin/bash
set -e

echo "⏳ Aguardando Kafka..."
sleep 30

echo "📌 Criando tópico accounts-events..."
kafka-topics \
  --bootstrap-server kafka:9092 \
  --create \
  --if-not-exists \
  --topic accounts-events \
  --partitions 1 \
  --replication-factor 1

echo "✅ Tópico accounts-events pronto"